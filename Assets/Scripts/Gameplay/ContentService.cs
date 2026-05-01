using TowerOblivion.Gameplay.Content;

namespace TowerOblivion.Gameplay
{
    public sealed class ContentService
    {
        public PlaceholderContentCatalogs Catalogs { get; }

        public ContentService(PlaceholderContentCatalogs catalogs)
        {
            Catalogs = catalogs;
        }
    }
}
