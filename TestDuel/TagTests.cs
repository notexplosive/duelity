using Duel.Data;
using FluentAssertions;
using System;
using Xunit;

namespace TestDuel
{
    public class TagTests
    {
        [Fact]
        public void can_add_retrieve_tags()
        {
            var subject = new TagCollection();
            subject.AddTag(new BlockProjectileTag());
            subject.HasTag<BlockProjectileTag>().Should().BeTrue();
        }

        [Fact]
        public void cannot_retrieve_tags_that_do_not_have()
        {
            var subject = new TagCollection();
            subject.HasTag<BlockProjectileTag>().Should().BeFalse();
            var caught = false;

            try
            {
                subject.GetTag<BlockProjectileTag>();
            }
            catch (Exception e)
            {
                e.Should().BeOfType<TagNotFoundException<BlockProjectileTag>>();
                caught = true;
            }

            caught.Should().BeTrue();
        }

        [Fact]
        public void can_try_and_fail_to_find_tag()
        {
            var subject = new TagCollection();
            var found = subject.TryGetTag<SolidTag>(out SolidTag solidTag);

            solidTag.Should().BeNull();
            found.Should().BeFalse();
        }

        [Fact]
        public void can_try_and_succeed_to_find_tag()
        {
            var subject = new TagCollection();
            subject.AddTag(new BlockProjectileTag());
            var found = subject.TryGetTag<BlockProjectileTag>(out BlockProjectileTag tag);

            tag.Should().NotBeNull();
            found.Should().BeTrue();
        }

        [Fact]
        public void cannot_duplicate_tags()
        {
            var subject = new TagCollection();
            subject.AddTag(new BlockProjectileTag());
            var caught = false;
            try
            {
                subject.AddTag(new BlockProjectileTag());
            }
            catch (Exception e)
            {
                e.Should().BeOfType<DuplicateTagException>();
                caught = true;
            }

            caught.Should().BeTrue();
        }

        [Fact]
        public void can_retrieve_specific_tag_info()
        {
            var subject = new TagCollection();
            subject.AddTag(new SolidTag().PushOnBump());
            subject.GetTag<SolidTag>().IsPushOnBump.Should().BeTrue();
        }
    }
}
