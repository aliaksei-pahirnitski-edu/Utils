using FileMerger.Domain.Entity;

namespace FilesHashComparer.Domain.Result
{
    public class MatchItemResult
    {
        public bool HasMatches { get; }

        /// <summary>Found matches in target snapshot</summary>
        public IEnumerable<string> FullPathes { get; }
        
        /// <summary>For which file this reult is </summary>
        public string FullPathOfSubject { get; }

        public MatchItemResult(string fullPathOfSubject, IEnumerable<ComparableEntity> matches)
        {
            if (matches == null) throw new ArgumentNullException(nameof(matches));
            // validation, but need also hostname : if (matches.Any(match => match.FullName == fullPathOfSubject ))

            FullPathOfSubject = fullPathOfSubject;
            
            FullPathes = matches.Select(x => x.FullName).ToList();
            HasMatches = FullPathes.Any();
        }
    }
}
