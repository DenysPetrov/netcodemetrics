﻿using CodeMetrics.Parsing;
using ICSharpCode.NRefactory.CSharp;

namespace CodeMetrics.Calculators
{
    public class BranchesVisitor : DepthFirstAstVisitor, IBranchesVisitor
    {
        public int BranchesCounter { get; protected set; }

        public override void VisitIfElseStatement(IfElseStatement ifElseStatement)
        {
            base.VisitIfElseStatement(ifElseStatement);

            BranchesCounter++;

            if (!ifElseStatement.FalseStatement.IsNull)
            {
                BranchesCounter++;
            }

            var conditionComplexity = GetConditionComplexity(ifElseStatement.Condition);
            BranchesCounter += conditionComplexity;

            
        }

        private static int GetConditionComplexity(Expression condition)
        {
            var branchesVisitorImpl = new ConditionVisitor();
            condition.AcceptVisitor(branchesVisitorImpl);
            return branchesVisitorImpl.BranchesCounter;
        }

        public override void VisitForStatement(ForStatement forStatement)
        {
            base.VisitForStatement(forStatement);

            BranchesCounter++;
            var conditionComplexity = GetConditionComplexity(forStatement.Condition);
            BranchesCounter += conditionComplexity;
            
        }

        public override void VisitForeachStatement(ForeachStatement foreachStatement)
        {
            base.VisitForeachStatement(foreachStatement);

            BranchesCounter++;
        }


        public override void VisitWhileStatement(WhileStatement whileStatement)
        {
            base.VisitWhileStatement(whileStatement);

            BranchesCounter++;

            var conditionComplexity = GetConditionComplexity(whileStatement.Condition);
            BranchesCounter += conditionComplexity;
        }

        public override void VisitDoWhileStatement(DoWhileStatement doLoopStatement)
        {
            base.VisitDoWhileStatement(doLoopStatement);

            BranchesCounter++;
            var conditionComplexity = GetConditionComplexity(doLoopStatement.Condition);
            BranchesCounter += conditionComplexity;
        }

        public override void VisitSwitchSection(SwitchSection switchSection)
        {
            base.VisitSwitchSection(switchSection);

            if (IsNotDefaultCase(switchSection))
                BranchesCounter++;
        }

        private static bool IsNotDefaultCase(SwitchSection switchSection)
        {
            var firstCaseLabel = switchSection.CaseLabels.FirstOrNullObject();
            return ((TokenRole)firstCaseLabel.FirstChild.Role).Token != "default";
        }
    }
}