using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPropietaria
{
    public class ConnectionDB
    {
        private ContabilidadEntities entities;
        private static ConnectionDB instance;

        private ConnectionDB() {
            entities = new ContabilidadEntities();
        }

        public void resetConnection() {
            entities.Dispose();
            entities = new ContabilidadEntities();
        }

        public static ConnectionDB getInstance() {
            if (instance == null)
            {
                instance = new ConnectionDB();
            }

            return instance;
        }

        public ContabilidadEntities getEntities() {
            return this.entities;
        }
    }
}
