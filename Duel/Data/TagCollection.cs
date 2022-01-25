using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Duel.Data
{
    public class TagCollection : IEnumerable<Tag>
    {
        private readonly List<Tag> content = new List<Tag>();

        public void AddTag(Tag tag)
        {
            if (HasTagSimilarTo(tag))
            {
                throw new DuplicateTagException(tag);
            }
            this.content.Add(tag);
        }

        public bool HasTagSimilarTo(Tag other)
        {
            foreach (var tag in this.content)
            {
                if (tag.GetType() == other.GetType())
                {
                    return true;
                }
            }

            return false;
        }

        public bool TryGetTag<T>(out T result) where T : Tag
        {
            foreach (var tag in this.content)
            {
                if (tag is T match)
                {
                    result = match;
                    return true;
                }
            }

            result = null;
            return false;
        }

        public bool HasTag<T>() where T : Tag
        {
            foreach (var tag in this.content)
            {
                if (tag is T)
                {
                    return true;
                }
            }

            return false;
        }

        public T GetTag<T>() where T : Tag
        {
            foreach (var tag in this.content)
            {
                if (tag is T result)
                {
                    return result;
                }
            }

            throw new TagNotFoundException<T>();
        }

        public IEnumerator<Tag> GetEnumerator()
        {
            return this.content.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.content.GetEnumerator();
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            foreach (var tag in this.content)
            {
                stringBuilder.Append('[');
                stringBuilder.Append(tag.ToString());
                stringBuilder.Append(']');
                stringBuilder.Append(' ');
            }

            return stringBuilder.ToString();
        }
    }
}
