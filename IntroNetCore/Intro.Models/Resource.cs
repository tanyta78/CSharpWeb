namespace Intro.Models
{
    public class Resource
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public ResourceType ResourceType { get; set; }

        public string Url { get; set; }

        public int CourceId { get; set; }

        public Course Course { get; set; }
    }
}