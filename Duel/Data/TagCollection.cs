using System.Collections.Generic;
using System.Diagnostics;

namespace Duel.Data
{
    public class TagCollection
    {
        private List<Tag> content = new List<Tag>();

        public void AddTag(Tag tag)
        {
            if (HasTagSimilarTo(tag))
            {
                throw new DuplicateTagException(tag);
            }
            this.content.Add(tag);
        }

        public bool HasTagSimilarTo(Tag example)
        {
            foreach (var tag in this.content)
            {
                if (tag.GetType() == example.GetType())
                {
                    return true;
                }
            }

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
    }
}
