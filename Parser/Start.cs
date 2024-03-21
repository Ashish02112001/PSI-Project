using Parser;

namespace PSI;

static class Start {
   static void Main () {
      var parser = new Parser (new Tokenizer (Expr0));
      var node = parser.Parse ();
      var dict = new Dictionary<string, int> () { ["five"] = 5, ["two"] = 2 };
      var sb = node.Accept (new ExprILGen ());
      ExprTyper et = new ();
      if (node.Accept (et) != NType.Error) {
         if (node.Accept (et) is NType.Bool) {
            ExprEvaluator expr = new (dict);
            bool result = node.Accept (expr) == 1;
            Console.WriteLine (result);
         }
         else {
            int value = node.Accept (new ExprEvaluator (dict));
            Console.WriteLine ($"Value = {value}");
         }
         ExprGrapher newGraph = new ();
         var n = node.Accept (newGraph);
         newGraph.WriteToFile (Expr0);
         Console.WriteLine ("\nGenerated code: ");
         Console.WriteLine (sb);
      }
      else Console.WriteLine ("Input a proper expression");
   }
   static string Expr0
      = "(3 + 2) * 4 - 17 * -5 * (2 + 1 + 4 + 5)";
}