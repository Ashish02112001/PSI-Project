namespace PSI;
using static Token.E;

// An expression evaluator, implementing using the visitor pattern
class ExprEvaluator : Visitor<int> {
   public ExprEvaluator (Dictionary<string, int> dict) => mDict = dict;
   Dictionary<string, int> mDict;

   public override int Visit (NLiteral literal)
      => int.Parse (literal.Value.Text);

   public override int Visit (NIdentifier identifier)
      => mDict[identifier.Name.Text];

   public override int Visit (NUnary unary) {
      int d = unary.Expr.Accept (this);
      if (unary.Op.Kind == SUB) d = -d;
      return d;
   }

   public override int Visit (NBinary binary) {
      int a = binary.Left.Accept (this), b = binary.Right.Accept (this);
      bool result = false;
     
      return binary.Op.Kind switch {
         ADD => a + b, SUB => a - b, MUL => a * b, DIV => a / b,
         EQ => Comparison (EQ), NEQ => Comparison(NEQ), LT => Comparison (LT), GT => Comparison (GT),
         LEQ => Comparison(LEQ), GEQ => Comparison (GEQ),
         _ => throw new NotImplementedException ()
      };
      int Comparison (Token.E token) {
         switch (token) {
            case EQ: result = a == b; break;
            case NEQ: result = a != b; break;
            case LT: result = a < b; break;
            case GT: result = a > b; break;
            case LEQ: result = a <= b; break;
            case GEQ: result = a >= b; break;
         }
         return result ? 1 : 0;
      }
   }
}
