using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PayrollSoft.Models
{
    /// <summary>
    /// AccessId = CRUD/CRU/CR/C etc
    /// Description: CRUD = CREATE+READ+UPDATE+DELETE
    /// </summary>
    
    public class AccessLevel
    {
        [Key]
        public string AccessId { get; set; } 
        public string Description { get; set; }  
    }
}