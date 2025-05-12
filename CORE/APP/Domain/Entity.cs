using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.APP.Domain
{
    public abstract class Entity
    {
        // Field to store the entity's ID.
        //private int id; // variable, field

        // Method to get the entity's ID.
        //public int GetId() // function, method, behavior
        //{
        //    return id;
        //}

        // Method to set the entity's ID.
        //public void SetId(int id)
        //{
        //    this.id = id;
        //}



        /// <summary>
        /// Gets or sets the ID of the entity.
        /// </summary>
        public virtual int Id { get; set; } // Property to get or set the entity's ID.

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class.
        /// </summary>
        protected Entity()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Entity"/> class with a specified ID.
        /// </summary>
        /// <param name="id">The ID of the entity.</param>
        protected Entity(int id) // Constructor to initialize the entity with an ID.
        {
            Id = id;
        }
    }
}
