using System.Reflection;

namespace Psycheflow.Api.Utils
{
    public static class UpdateEntityHandler
    {
        // método que pega as propriedades de um dto e as propriedades de uma entidade e as preenche dinâmicamente com reflection
        public static void Update<TDto, TEntity>(TDto dto, TEntity entity)
        {
            PropertyInfo[] dtoProps = typeof(TDto).GetProperties();
            PropertyInfo[] entityProps = typeof(TEntity).GetProperties();

            foreach (PropertyInfo dtoProp in dtoProps)
            {
                object? value = dtoProp.GetValue(dto);
                if (value is not null)
                {
                    PropertyInfo? entityProp = entityProps.FirstOrDefault(p => p.Name == dtoProp.Name && p.PropertyType == dtoProp.PropertyType);
                    if (entityProp is not null)
                    {
                        entityProp.SetValue(entity, value);
                    }
                }
            }
        }

    }
}
