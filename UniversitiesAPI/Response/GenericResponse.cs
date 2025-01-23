// author: feldy judah k
// .NET 8

namespace UniversitiesAPI.Response
{
    public class GenericResponse<TEntity> where TEntity : class
    {
        public string Middleware { get; set; } 

        public List<TEntity> Entity { get; set; }
    }
}
