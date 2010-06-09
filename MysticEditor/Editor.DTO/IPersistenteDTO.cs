
namespace Editor.DTO {
    public interface IPersistenteDTO {
        /// <summary>
        /// Elemento creato nuovo
        /// </summary>
        bool IsNew { get; set; }
        /// <summary>
        /// Elemento modificato
        /// </summary>
        bool Dirty { get; set; }
        /// <summary>
        /// Elemento eliminato
        /// </summary>
        bool Deleted { get; set; }
        /// <summary>
        /// Elemento persistito
        /// </summary>
        bool IsPersisted { get; set; } 
    }


}
