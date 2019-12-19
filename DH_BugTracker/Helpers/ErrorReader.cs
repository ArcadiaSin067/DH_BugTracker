using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace DH_BugTracker.Helpers
{
    public class ErrorReader
    {
        public static string ErrorCompiler(ModelStateDictionary modelState)
        {
            var errorMsg = new StringBuilder();
            var allErrors = modelState.Where(x => x.Value.Errors.Count > 0)
                                      .Select(x => new { x.Key, x.Value.Errors }).ToList();
            foreach (var error in allErrors.SelectMany(kvPair => kvPair.Errors.Select(error => error)))
            {
                errorMsg.Append($"{error.ErrorMessage}|");
            }
            return errorMsg.ToString();
        }

    }
}