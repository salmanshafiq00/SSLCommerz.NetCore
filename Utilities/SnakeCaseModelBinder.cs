using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using SSLCommerz.NetCore.SSLCommerz;

namespace SSLCommerz.NetCore.Utilities;

public class SnakeCaseModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext bindingContext)
    {
        if (bindingContext == null)
        {
            throw new ArgumentNullException(nameof(bindingContext));
        }

        var formCollection = bindingContext.HttpContext.Request.Form;

        // Extract form data
        var formData = formCollection
            .ToDictionary(k => k.Key, v => v.Value.ToString());

        // Serialize to JSON using Newtonsoft.Json
        var jsonString = JsonConvert.SerializeObject(formData);

        // Get the target type from the action's parameter
        var targetType = bindingContext.ModelType;

        bindingContext.Result = ModelBindingResult.Success(JsonConvert.DeserializeObject(jsonString, targetType));

        return Task.CompletedTask;
    }
}
