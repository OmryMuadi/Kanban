using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DTO
    {
        protected DAO controller;


        //constructor
        protected DTO(DAO controller)
        {
            this.controller = controller;
        }
    }
}
