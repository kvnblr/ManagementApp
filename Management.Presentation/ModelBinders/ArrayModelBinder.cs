using System.ComponentModel;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Management.Presentation.ModelBinders;

public class ArrayModelBinder : IModelBinder
{
    public Task BindModelAsync(ModelBindingContext context)
    {
        if (!context.ModelMetadata.IsEnumerableType)
        {
            context.Result = ModelBindingResult.Failed();
            return Task.CompletedTask;
        }

        var providedValue = context.ValueProvider
            .GetValue(context.ModelName)
            .ToString();

        if (string.IsNullOrEmpty(providedValue))
        {
            context.Result = ModelBindingResult.Success(null);
            return Task.CompletedTask;
        }

        var genericType = context.ModelType.GetTypeInfo().GenericTypeArguments[0];
        var converter = TypeDescriptor.GetConverter(genericType);
        var objectArray = providedValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
            .Select(o => converter.ConvertFromString(o.Trim()))
            .ToArray();
        var guidArray = Array.CreateInstance(genericType, objectArray.Length);
        objectArray.CopyTo(guidArray, 0);
        context.Model = guidArray;

        context.Result = ModelBindingResult.Success(context.Model);
        return Task.CompletedTask;
    }
}
