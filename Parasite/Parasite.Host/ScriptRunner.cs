using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parasite.Host
{
    public class ScriptRunner
    {
        public async static Task<string> RunCode(string msg)
        {
			try
			{
				//var globals = new TestVariables();

				var sopts = ScriptOptions.Default;
				sopts = sopts.WithImports("System", "System.Collections.Generic", "System.Linq", "System.Text", "System.Threading.Tasks", "System.IO");
				sopts = sopts.WithReferences(AppDomain.CurrentDomain.GetAssemblies().Where(xa => !xa.IsDynamic && !string.IsNullOrWhiteSpace(xa.Location)));

				var script = CSharpScript.Create(msg, sopts/*, typeof(TestVariables)*/);
				script.Compile();
				var result = await script.RunAsync(null).ConfigureAwait(false);

				if (result != null && result.ReturnValue != null && !string.IsNullOrWhiteSpace(result.ReturnValue.ToString()))
					return $"Result:\n{result.ReturnValue.ToString()}";
				else
					return $"Result: Success";
			}
			catch (Exception ex)
			{
				return $"Result: Fail\n{string.Concat(ex.GetType().ToString(), ": ", ex.Message)}";
			}
		}
    }
}
