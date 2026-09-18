using ArgosSharp.Domain.ValueObjects;

namespace ArgosSharp.Infrastructure.Mapper
{
    public static class JobPostingMapper
    {
        public static JobPosting Map(
            string title,
            string link,
            string description,
            string source)
        {
            return new JobPosting(Title: title,Link: link,Description: description,Source: source);
        }
    }
}
