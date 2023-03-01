using Ardalis.GuardClauses;

namespace Driver.Models
{
    public class FileInfoModel : IComparable<FileInfoModel>
    {
        public bool Exists { get; set; }
        public long Length { get; set; }
        public string? PhysicalPath { get; set; }
        public string Name { get; set; } = default!;
        public DateTimeOffset ModifiedOnUtc { get; set; }
        public bool IsDirectory { get; set; }

        public int CompareTo(FileInfoModel? other)
        {
            Guard.Against.Null(other);
            return Name.CompareTo(other.Name);
        }
    }
}
