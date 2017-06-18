using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoPropietaria.Models
{
    public class User
    {
        public String name { get; set; }
        public int permission { get; set; }

        public User() { }
        public User(String name, int permission) {
            this.name = name;
            this.permission = permission;
        }
    }
}
