using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPropietaria.Models
{
    public class User
    {
        public int id { get; set; }
        public String name { get; set; }
        public int permission { get; set; }

        public User() { }
        public User(int id, String name, int permission) {
            this.id = id;
            this.name = name;
            this.permission = permission;
        }

        public User(String name, int permission) {
            this.name = name;
            this.permission = permission;
        }
    }
}
