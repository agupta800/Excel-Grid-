using System.Collections.Generic;

namespace AsterWebApp.Models
{
    public class MenuModel
    {
        public long MenuId { get; set; }

        public string Name { get; set; } = null!;

        public string? IconClass { get; set; }

        public short DisplayOrder { get; set; }

        public short AreaId { get; set; }

        public virtual ICollection<SubMenuModel> SubMenuModels { get; } = new List<SubMenuModel>();
    }

    public partial class ChildSubMenuModel
    {
        public long ChildSubMenuId { get; set; }

        public string Name { get; set; } = null!;

        public short DisplayOrder { get; set; }

        public string ControllerName { get; set; } = null!;

        public string ViewName { get; set; } = null!;

        public string AreaName { get; set; } = null!;

        public short AreaId { get; set; }

        public long SubMenuId { get; set; }
        public virtual SubMenuModel SubMenus { get; set; } = null!;
    }

    public partial class SubMenuModel
    {
        public long SubMenuId { get; set; }

        public string Name { get; set; } = null!;

        public short DisplayOrder { get; set; }

        public string ControllerName { get; set; } = null!;

        public string ViewName { get; set; } = null!;

        public string AreaName { get; set; } = null!;

        public short AreaId { get; set; }

        public long MenuId { get; set; }

        public virtual ICollection<ChildSubMenuModel> ChildSubMenus { get; } = new List<ChildSubMenuModel>();

        public virtual MenuModel Menu { get; set; } = null!;
    }

    public partial class HirercySubMenuModel
    {
        public long SubMenuId { get; set; }

        public string Name { get; set; } = null!;

        public short DisplayOrder { get; set; }

        public string ControllerName { get; set; } = null!;

        public string ViewName { get; set; } = null!;

        public string? AreaName { get; set; }

        public short AreaId { get; set; }

        public long MenuId { get; set; }
    }

    public partial class HirercyMenuModel
    {
        public long MenuId { get; set; }

        public string Name { get; set; } = null!;

        public string? IconClass { get; set; }

        public int DisplayOrder { get; set; }

        public int AreaId { get; set; }
    }
}
