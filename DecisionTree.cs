using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    [System.Serializable]
    public struct Attributes
    {
        public float percentHealth;
        public bool prevAttack;
        public bool prevPlayerAttack;
        public bool shouldAttack;
        public Attributes(float percentHealth, bool prevAttack, bool prevPlayerAttack)
        {
            this.percentHealth = percentHealth;
            this.prevAttack = prevAttack;
            this.prevPlayerAttack = prevPlayerAttack;
            shouldAttack = false;
        }
        public Attributes(float percentHealth, bool prevAttack, bool prevPlayerAttack, bool shouldAttack)
        {
            this.percentHealth = percentHealth;
            this.prevAttack = prevAttack;
            this.prevPlayerAttack = prevPlayerAttack;
            this.shouldAttack = shouldAttack;
        }
    }
    [System.Serializable]
    public struct Rule
    {
        public bool expression;
        public bool used;
        public Rule(ref bool expression)
        {
            this.expression = expression;
            used = false;
        }
    }
    private class Node
    {
        public Node left, right;
        public Stack<Attributes> attributes = new Stack<Attributes>();
        public bool rule;
        public bool shouldAttack = false;
        public bool isLeaf;
        public int level = 0;
        public Node(bool shouldAttack)
        {
            this.shouldAttack = shouldAttack;
            isLeaf = true;
        }
        public Node(Stack<Attributes> attributes, int level)
        {
            this.attributes = attributes;
            this.level = level;
            isLeaf = false;
        }
        public void SetRule(ref bool rule)
        {
            this.rule = rule;
        }
    }
    public static Attributes currentState = new Attributes();
    public static bool[] ruleExpressions = { currentState.percentHealth > 0.6f, currentState.percentHealth == 1, currentState.percentHealth < 0.5, currentState.prevPlayerAttack, currentState.prevAttack };
    [SerializeField] private Rule[] rules = { new Rule(ref ruleExpressions[0]), new Rule(ref ruleExpressions[1]), new Rule(ref ruleExpressions[2]), new Rule(ref ruleExpressions[3]), new Rule(ref ruleExpressions[4]) };
    private Queue<Rule> ruleQueue = new Queue<Rule>();
    private Node startingNode = new Node(new Stack<Attributes>(), 0);
    [SerializeField] int levelCutoff = 4;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Rule curRule in rules)
        {
            ruleQueue.Enqueue(curRule);
        }
        //                                          HP   enemy  player  attack?
        startingNode.attributes.Push(new Attributes(0.2f, false, true, false));
        startingNode.attributes.Push(new Attributes(0.4f, true, false, false));
        startingNode.attributes.Push(new Attributes(0.6f, true, true, true));
        startingNode.attributes.Push(new Attributes(0.2f, true, false, false));
        startingNode.attributes.Push(new Attributes(0.2f, false, false, true));
        startingNode.attributes.Push(new Attributes(0.4f, false, false, false));
        startingNode.attributes.Push(new Attributes(1f, false, true, true));
        startingNode.attributes.Push(new Attributes(1f, true, false, true));
        startingNode.attributes.Push(new Attributes(1f, false, false, true));
        startingNode.attributes.Push(new Attributes(1f, true, true, true));
        startingNode.attributes.Push(new Attributes(0.6f, true, true, false));
        startingNode.attributes.Push(new Attributes(0.2f, false, false, false));
        startingNode.attributes.Push(new Attributes(0.8f, false, true, true));
        startingNode.attributes.Push(new Attributes(0.8f, true, true, true));
        startingNode.attributes.Push(new Attributes(0.8f, true, false, true));



        startingNode.attributes.Push(new Attributes(0.8f, false, true, false));
        startingNode.attributes.Push(new Attributes(0.8f, true, true, false));
        startingNode.attributes.Push(new Attributes(0.8f, true, false, false));
        startingNode.attributes.Push(new Attributes(1f, false, false, false));
        startingNode.attributes.Push(new Attributes(1f, true, true, false));
        startingNode.attributes.Push(new Attributes(0.6f, true, true, false));
        startingNode.attributes.Push(new Attributes(0.2f, false, false, false));
        BuildTree(startingNode);
    }

    void BuildTree(Node parent)
    {
        if(CalculateEntropy(parent.attributes) == 0)
        {
            parent.isLeaf = true;
            parent.shouldAttack = parent.attributes.Peek().shouldAttack;
            Debug.Log(parent.shouldAttack);
            return;
        }
        else if(parent.level >= levelCutoff)
        {
            parent.isLeaf = true;
            int trueCount = 0;
            int totalCount = parent.attributes.Count;
            for(int i=0; i<totalCount; i++)
            {
                if(parent.attributes.Pop().shouldAttack)
                {
                    trueCount++;
                }
            }
            parent.shouldAttack = (trueCount >= totalCount - trueCount);
            Debug.Log(parent.shouldAttack);
            return;
        }
        Stack<Attributes> toLeft = new Stack<Attributes>();
        Stack<Attributes> toRight = new Stack<Attributes>();
        Attributes[] curAttributes = parent.attributes.ToArray();
        float maxInfoGain = 0;
        Rule bestRule = ruleQueue.Peek();
        for (int i=0; i<ruleQueue.Count; i++)
        {
            Rule curRule = ruleQueue.Dequeue();
            if(curRule.used)
            {
                continue;
            }
            ruleQueue.Enqueue(curRule);
            foreach(Attributes attribute in curAttributes)
            {
                currentState = attribute;
                if(curRule.expression)
                {
                    toRight.Push(attribute);
                }
                else
                {
                    toLeft.Push(attribute);
                }
            }
            float infoGain = CalculateInfoGain(CalculateEntropy(parent.attributes), curAttributes.Length, CalculateEntropy(toLeft), toLeft.Count, CalculateEntropy(toRight), toRight.Count);
            if(infoGain > maxInfoGain)
            {
                maxInfoGain = infoGain;
                bestRule = curRule;
            }
        }
        if(toLeft.Count == 0 || toRight.Count == 0)
        {
            parent.isLeaf = true;
            parent.shouldAttack = parent.attributes.Peek().shouldAttack;
            return;
        }
        parent.SetRule(ref bestRule.expression);
        Debug.Log("Going left");
        parent.left = new Node(toLeft, parent.level + 1);
        BuildTree(parent.left);
        Debug.Log("Going right");
        parent.right = new Node(toRight, parent.level + 1);
        BuildTree(parent.right);
    }

    float CalculateEntropy(Stack<Attributes> values)
    {
        Attributes[] list = values.ToArray();
        int trueCount = 0;
        foreach(Attributes attribute in list)
        {
            if (attribute.shouldAttack)
                trueCount++;
        }
        int falseCount = values.Count - trueCount;
        return 1 - (Mathf.Pow(((float)trueCount / values.Count), 2) + Mathf.Pow(((float)falseCount / values.Count), 2));
    }

    float CalculateInfoGain(float parentEntropy, int parentCount, float leftEntropy, int leftCount, float rightEntropy, int rightCount)
    {
        return parentEntropy - ((leftEntropy * (float)leftCount / parentCount) + (rightEntropy * (float)rightCount / parentCount));
    }

    public bool ShouldAttack(float percentHealth, bool prevAttack, bool prevPlayerAttack)
    {
        currentState = new Attributes(percentHealth, prevAttack, prevPlayerAttack);
        Debug.Log("Enemy Health: " + percentHealth);
        if(startingNode.rule)
        {
            return ShouldAttack(startingNode.right);
        }
        else
        {
            return ShouldAttack(startingNode.left);
        }
    }

    private bool ShouldAttack(Node curNode)
    {
        if(curNode.isLeaf)
        {
            return curNode.shouldAttack;
        }
        if (curNode.rule)
        {
            return ShouldAttack(curNode.right);
        }
        else
        {
            return ShouldAttack(curNode.left);
        }
    }
}
