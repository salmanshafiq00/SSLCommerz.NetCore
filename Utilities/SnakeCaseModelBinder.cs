using Microsoft.AspNetCore.Mvc.ModelBinding;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

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

        // Get the target type from the action's parameter
        var targetType = bindingContext.ModelType;

        try
        {
            // Serialize to JSON using Newtonsoft.Json with custom settings
            var jsonString = JsonConvert.SerializeObject(formData);

            // Deserialize to the target type
            var result = JsonConvert.DeserializeObject(jsonString, targetType);

            // Set the result in the binding context
            bindingContext.Result = ModelBindingResult.Success(result);
        }
        catch (Exception ex)
        {
            // Handle any exception during binding
            bindingContext.ModelState.AddModelError(bindingContext.ModelName, ex.Message);
        }

        return Task.CompletedTask;
    }
}
