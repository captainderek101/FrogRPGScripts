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
    class Node
    {
        public Node left, right;
        public Stack<Attributes> attributes = new Stack<Attributes>();
        public bool rule;
        public bool shouldAttack;
        public bool isLeaf;
        public Node(bool shouldAttack)
        {
            this.shouldAttack = shouldAttack;
            isLeaf = true;
        }
        public Node(Stack<Attributes> attributes)
        {
            this.attributes = attributes;
            isLeaf = false;
        }
        public void SetRule(ref bool rule)
        {
            this.rule = rule;
        }
    }
    public static Attributes currentState = new Attributes();
    public static bool[] ruleExpressions = { currentState.percentHealth > 0.5f, currentState.prevPlayerAttack, currentState.prevAttack };
    [SerializeField] private Rule[] rules = { new Rule(ref ruleExpressions[0]), new Rule(ref ruleExpressions[1]), new Rule(ref ruleExpressions[2]) };
    private Queue<Rule> ruleQueue;
    private Node startingNode;
    // Start is called before the first frame update
    void Start()
    {
        foreach(Rule curRule in rules)
        {
            ruleQueue.Enqueue(curRule);
        }
        BuildTree(startingNode);
    }

    void BuildTree(Node parent)
    {
        if(CalculateEntropy(parent.attributes) == 0)
        {
            parent.isLeaf = true;
            parent.shouldAttack = parent.attributes.Peek().shouldAttack;
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
        parent.SetRule(ref bestRule.expression);
        parent.left = new Node(toLeft);
        BuildTree(parent.left);
        parent.right = new Node(toRight);
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
}
