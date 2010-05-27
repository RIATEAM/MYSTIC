using System.Collections.Generic;
using Editor.BL;
using Editor.BE.Model;
using Editor.DTO;
using AutoMapper;


namespace Editor.Services {
    public class ContentServices {

        public IList<ContentDTO> GetContents() {
            IList<Content> List = new List<Content>();
            List = EditorServices.GetContents<Content>() as List<Content>;
            Mapper.CreateMap<Content, ContentDTO>();
            return Mapper.Map<IList<Content>,IList<ContentDTO>>(List);
        }
    }
}
