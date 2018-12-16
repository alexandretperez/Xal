namespace Xal.Util
{
    /// <summary>
    /// A utility class to work with objects in general.
    /// </summary>
    public static class ObjectUtils
    {
        /// <summary>
        /// Copies the property values from <paramref name="source"/> object to specified <paramref name="target"/> object when the types are assignable.
        /// </summary>
        /// <typeparam name="T">The target type.</typeparam>
        /// <param name="target">The target object.</param>
        /// <param name="source">The source object.</param>
        public static object CopyValuesTo<T>(T target, object source) where T : class
        {
            var sourceType = source.GetType();
            foreach (var prop in target.GetType().GetProperties())
            {
                if (!prop.CanWrite)
                    continue;

                var sourceProp = sourceType.GetProperty(prop.Name);
                if (sourceProp == null)
                    continue;

                if (!prop.PropertyType.IsAssignableFrom(sourceProp.PropertyType))
                    continue;

                prop.SetValue(target, sourceProp.GetValue(source, null), null);
            }

            return target;
        }
    }
}