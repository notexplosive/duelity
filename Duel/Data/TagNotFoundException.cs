using System;
using System.Runtime.Serialization;

namespace Duel.Data
{
    [Serializable]
    public class DuplicateTagException : Exception
    {
        public DuplicateTagException(Tag tag) : base($"Object already has a {tag.GetType().Name}")
        {
        }
    }

    [Serializable]
    public class TagNotFoundException<T> : Exception where T : Tag
    {
        public TagNotFoundException() : base($"{nameof(T)} not found")
        {
        }

        public TagNotFoundException(Exception innerException) : base($"{nameof(T)} not found", innerException)
        {
        }
    }
}