using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Domain
{
    public class AccessRights
    {
        public int FromId { get; set; } // Идентификатор пользователя, предоставившего доступ к своей аналитике. Входит в состав первичного ключа
        
        [ForeignKey("FromId")]
        public User From { get; set; }
        public int ToId { get; set; } // Идентификатор пользователя, получившего доступ к аналитике другого пользователя. Входит в состав первичного ключа.

        [ForeignKey("ToId")]
        public User To { get; set; }
    }
}
