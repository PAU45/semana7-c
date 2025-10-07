using System.Collections.Generic;
using Data;
using Entity;

namespace Business
{
    public class BProduct
    {
        private readonly DProduct _data = new DProduct();

        public List<Product> Read() => _data.Read();
        public Product? Get(int id) => _data.Get(id);
        public int Insert(Product p) => _data.Insert(p);
        public bool Update(Product p) => _data.Update(p);
        public bool Delete(int id) => _data.Delete(id); // eliminación lógica
    }
}
