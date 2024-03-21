using PSI;
using static PSI.NType;

namespace Parser {
   class ExprTyper : Visitor<NType> {
      public override NType Visit (NLiteral literal) {
         var txt = literal.Value.Text;
         if (txt.Contains ('.') || txt.Contains ('e') || txt.Contains ('E')) literal.Type = Real;
         else if (txt is "true" or "false") literal.Type = Bool;
         else literal.Type = Int;
         return literal.Type;
      }

      public override NType Visit (NIdentifier ident) {
         return ident.Type = Unknown;
      }

      public override NType Visit (NUnary unary) {
         return unary.Type = unary.Expr.Accept (this);
      }

      public override NType Visit (NBinary binary) {
         var a = binary.Left.Accept (this);
         var b = binary.Right.Accept (this);
         if (binary.Op.Text is "<" or ">" or ">=" or "<=" or "<>" or "=" && (a == b || a is Bool || b is Bool) ) binary.Type = Bool;
         else if (a is Int && b is Int) binary.Type = Int;
         else if (a is Int or Real && b is Real or Int) binary.Type = Real;
         else binary.Type = Error;
         return binary.Type;
      }
   }
}