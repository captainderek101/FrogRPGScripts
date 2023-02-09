using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecisionTree : MonoBehaviour
{
    // Attribute struct for use in learning and input for the decision trees
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
    // Rule Struct for use setting expressions by reference
    [System.Serializable]
    public class Rule
    {
        public int numExpressions = 5;
        public Attributes attribute;
        public bool expression;
        public bool used;
        List<System.Action> ruleList = new List<System.Action>();
        public Rule()
        {
        }

        //public Rule(ref bool expression)
        //{
        //    this.expression = expression;
        //    used = false;
        //}
        public bool Evaluate(ref Attributes attribute, int expressionNum)
        {
            switch(expressionNum)
            {
                case 0:
                    return attribute.percentHealth > 0.6f;
                case 1:
                    return attribute.percentHealth == 1;
                case 2:
                    return attribute.percentHealth < 0.5f;
                case 3:
                    return attribute.prevPlayerAttack;
                case 4:
                    return attribute.prevAttack;
                default:
                    return used;
            }
        }
    }
    // Node class for the binary decision tree
    private class Node
    {
        public Node left, right;
        public Stack<Attributes> attributes = new Stack<Attributes>();
        public Rule rule;
        public int expressionNum;
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
        public void SetRule(Rule rule)
        {
            this.rule = rule;
        }
    }
    public static Attributes currentState = new Attributes();
    //public static bool[] ruleExpressions = { currentState.percentHealth > 0.6f, currentState.percentHealth == 1, currentState.percentHealth < 0.5f, currentState.prevPlayerAttack, currentState.prevAttack };
    //[SerializeField] private Rule[] rules = { new Rule(ref ruleExpressions[0]), new Rule(ref ruleExpressions[1]), new Rule(ref ruleExpressions[2]), new Rule(ref ruleExpressions[3]), new Rule(ref ruleExpressions[4]) };
    public Rule rule = new Rule();
    //private Queue<Rule> ruleQueue = new Queue<Rule>();
    private Node startingNode = new Node(new Stack<Attributes>(), 0);
    [SerializeField] int levelCutoff = 4;

    // Start is called before the first frame update
    void Start()
    {
        //foreach(Rule curRule in rules)
        //{
        //    ruleQueue.Enqueue(curRule);
        //}
        //                                          HP   enemy  player  attack?
        startingNode.attributes.Push(new Attributes(0.2f, false, true, true));
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
        currentState.percentHealth = 0.8f;
        Debug.Log(rule.Evaluate(ref currentState, 0));
        currentState.percentHealth = 0.4f;
        Debug.Log(rule.Evaluate(ref currentState, 0));


        //startingNode.attributes.Push(new Attributes(0.8f, false, true, false));
        //startingNode.attributes.Push(new Attributes(0.8f, true, true, false));
        //startingNode.attributes.Push(new Attributes(0.8f, true, false, false));
        //startingNode.attributes.Push(new Attributes(1f, false, false, false));
        //startingNode.attributes.Push(new Attributes(1f, true, true, false));
        //startingNode.attributes.Push(new Attributes(0.6f, true, true, false));
        //startingNode.attributes.Push(new Attributes(0.2f, false, false, false));


        BuildTree(startingNode);
    }

    // recursive function that builds the decision tree with a pre-order traversal
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
        //Rule bestRule = ruleQueue.Peek();
        int bestRule = 0;
        for (int i=0; i<rule.numExpressions; i++)
        {
            //Rule curRule = ruleQueue.Dequeue();
            //if(curRule.used)
            //{
            //    continue;
            //}
            //ruleQueue.Enqueue(curRule);
            foreach(Attributes attribute in curAttributes)
            {
                currentState = attribute;
                //Debug.Log(i + ": " + curRule.expression);
                if (rule.Evaluate(ref currentState, i))
                {
                    //Debug.Log("Going right");
                    toRight.Push(attribute);
                }
                else
                {
                    toLeft.Push(attribute);
                }
            }
            if (toLeft.Count != 0 && toRight.Count != 0)
            {
                float infoGain = CalculateInfoGain(CalculateEntropy(parent.attributes), curAttributes.Length, CalculateEntropy(toLeft), toLeft.Count, CalculateEntropy(toRight), toRight.Count);
                Debug.Log(infoGain);
                if (infoGain > maxInfoGain)
                {
                    maxInfoGain = infoGain;
                    bestRule = i;
                }
            }
            toLeft.Clear();
            toRight.Clear();
        }
        foreach (Attributes attribute in curAttributes)
        {
            currentState = attribute;
            if (rule.Evaluate(ref currentState, parent.expressionNum))
            {
                toRight.Push(attribute);
            }
            else
            {
                //Debug.Log("Going left");
                toLeft.Push(attribute);
            }
        }
        //if (toLeft.Count == 0 || toRight.Count == 0 || maxInfoGain == 0)
        //{
        //    parent.isLeaf = true;
        //    int trueCount = 0;
        //    int totalCount = parent.attributes.Count;
        //    for (int i = 0; i < totalCount; i++)
        //    {
        //        if (parent.attributes.Pop().shouldAttack)
        //        {
        //            trueCount++;
        //        }
        //    }
        //    parent.shouldAttack = (trueCount >= totalCount - trueCount);
        //    Debug.Log(parent.shouldAttack);
        //    return;
        //}
        //bestRule.used = true;
        parent.SetRule(rule);
        parent.expressionNum = bestRule;
        Debug.Log("Going left");
        parent.left = new Node(toLeft, parent.level + 1);
        BuildTree(parent.left);
        Debug.Log("Going right");
        parent.right = new Node(toRight, parent.level + 1);
        BuildTree(parent.right);

        PrintTree(startingNode);
    }

    // returns entropy / Gini Impurity
    float CalculateEntropy(Stack<Attributes> values)
    {
        Attributes[] list = values.ToArray();
        int trueCount = 0;
        foreach(Attributes attribute in list)
        {
            if (attribute.shouldAttack)
                trueCount++;
        }
        int falseCount = list.Length - trueCount;
        float entropy = 1 - (Mathf.Pow(((float)trueCount / values.Count), 2) + Mathf.Pow(((float)falseCount / values.Count), 2));
        Debug.Log("Entropy = " + entropy);
        return entropy;
    }

    // Returns info gain for a rule
    float CalculateInfoGain(float parentEntropy, int parentCount, float leftEntropy, int leftCount, float rightEntropy, int rightCount)
    {
        return parentEntropy - ((leftEntropy * ((float)leftCount / parentCount)) + (rightEntropy * ((float)rightCount / parentCount)));
    }

    // Input for the decision tree. Input the current state of the enemy/battle
    public bool ShouldAttack(float percentHealth, bool prevAttack, bool prevPlayerAttack)
    {
        currentState = new Attributes(percentHealth, prevAttack, prevPlayerAttack);
        //Debug.Log("Enemy Health: " + percentHealth);
        if(startingNode.rule.Evaluate(ref currentState, startingNode.expressionNum))
        {
            return ShouldAttack(startingNode.right, currentState);
        }
        else
        {
            return ShouldAttack(startingNode.left, currentState);
        }
    }

    // Helper/recursive function for decision tree input
    private bool ShouldAttack(Node curNode, Attributes curState)
    {
        if(curNode.isLeaf)
        {
            return curNode.shouldAttack;
        }
        if (curNode.rule.Evaluate(ref curState, curNode.expressionNum))
        {
            return ShouldAttack(curNode.right, curState);
        }
        else
        {
            return ShouldAttack(curNode.left, curState);
        }
    }

    private void PrintTree(Node curNode)
    {
        if(curNode.isLeaf)
        {
            //print("Leaf with " + curNode.rule);
        }
        else
        {
            //print("Node with " + curNode.rule+" goes left to ");
            if(curNode.left != null)
                PrintTree(curNode.left);
            //print(" and right to ");
            if (curNode.right != null)
                PrintTree(curNode.right);
        }
    }
}
