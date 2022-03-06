﻿using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Wallet.ModelBinders
{
    public class DoubleModelBinderProvider : IModelBinderProvider
    {
        public IModelBinder? GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (context.Metadata.ModelType == typeof(Double) || context.Metadata.ModelType == typeof(Double?))
            {
                return new DoubleModelBinder();
            }

            return null;
        }
    }
}
