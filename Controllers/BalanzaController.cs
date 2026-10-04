using SER_Balanza_Interno.Data;
using SER_Balanza_Interno.Models;

namespace SER_Balanza_Interno.Controllers
{
    public class BalanzaController
    {
        public List<balanza> ObtenerActivas()
        {
            using var uow = UnitOfWorkFactory.Create();
            return uow.Repository<balanza>().Query()
                .Where(b => b.activo == true)
                .OrderBy(b => b.Descripcion)
                .ToList();
        }
    }
}
