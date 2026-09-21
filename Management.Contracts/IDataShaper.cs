using System.Dynamic;
using Management.Entities.Models;

namespace Management.Contracts;

public interface IDataShaper<T>
{
    IEnumerable<ExpandoObject> ShapeData(IEnumerable<T> entities, string fieldString);
    ExpandoObject ShapeData(T entity, string fieldString);

    IEnumerable<ShapedEntity> ShapedData(IEnumerable<T> entities, string fieldString);
    ShapedEntity ShapedData(T entity, string fieldString);
}
