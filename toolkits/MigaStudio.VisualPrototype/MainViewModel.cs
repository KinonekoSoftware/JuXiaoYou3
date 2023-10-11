using System;
using Acorisoft.FutureGL.Forest.ViewModels;
using GraphShape.Controls;
using QuikGraph;

namespace Acorisoft.FutureGL.MigaStudio.Repairs
{
    public class Person : IEquatable<Person>
    {
        public string Id { get; init; }
        public string Name { get; init; }

        public bool Equals(Person other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Id == other.Id && Name == other.Name;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && Equals((Person)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, Name);
        }
        public override string ToString() => Name;
    }

    public class Rel : IEdge<Person>
    {
        public string Name { get; init; }
        public Person Source { get; init; }
        public Person Target { get; init; }
        public override string ToString() => Name;
    }

    public class PersonGraph : BidirectionalGraph<Person, Rel>
    {
        
    }

    public class PersonGraphLayout : GraphLayout<Person, Rel, PersonGraph>
    {
        
    }

    public class MainViewModel : ViewModelBase
    {
        public MainViewModel()
        {
            Graph = new PersonGraph();
        }

        public void CreateCharacterRelationship()
        {
            var gf1 = new Person
            {
                Id   = "爷爷1",
                Name = "爷爷1",
            };
            var gf2 = new Person
            {
                Id   = "爷爷2",
                Name = "爷爷2",
            };
            var gm1 = new Person
            {
                Id   = "奶奶1",
                Name = "奶奶1",
            };
            var gm2 = new Person
            {
                Id   = "奶奶2",
                Name = "奶奶2",
            };
            var bobo = new Person
            {
                Id   = "伯伯",
                Name = "伯伯",
            };
            var shushu = new Person
            {
                Id   = "叔叔",
                Name = "叔叔",
            };
            var father = new Person
            {

                Id   = "父亲",
                Name = "父亲",
            };
            var gugu = new Person
            {

                Id   = "姑姑",
                Name = "姑姑",
            };
            var mother = new Person
            {

                Id   = "母亲",
                Name = "母亲",
            };
            
            var Ayi = new Person
            {

                Id   = "阿姨",
                Name = "阿姨",
            };
            
            var son = new Person
            {
                Id   = "儿子",
                Name = "儿子",
            };
            
            var daughter = new Person
            {
                Id   = "女儿",
                Name = "女儿",
            };

            var rel_grand_1 = new Rel
            {
                Source = gf1,
                Target = gm1,
                Name   = "夫妻"
            };
            var rel_grand_2 = new Rel
            {
                Source = gf2,
                Target = gm2,
                Name   = "夫妻"
            };
            
            Graph.Clear();
            Graph.AddVertex(gf1);
            Graph.AddVertex(gf2);
            Graph.AddVertex(gm1);
            Graph.AddVertex(gm2);
            Graph.AddVertex(bobo);
            Graph.AddVertex(father);
            Graph.AddVertex(shushu);
            Graph.AddVertex(gugu);
            Graph.AddVertex(mother);
            Graph.AddVertex(Ayi);
            Graph.AddVertex(son);
            Graph.AddVertex(daughter);
            Graph.AddEdge(rel_grand_1);
            Graph.AddEdge(rel_grand_2);
            Graph.AddEdge(new Rel
            {
                Source = gf1,
                Name = "父子",
                Target = bobo
            });
            Graph.AddEdge(new Rel
            {
                Source = gf1,
                Name   = "父子",
                Target = father
            });
            Graph.AddEdge(new Rel
            {
                Source = gf1,
                Name   = "父子",
                Target = shushu
            });
            Graph.AddEdge(new Rel
            {
                Source = gf1,
                Name   = "父女",
                Target = gugu
            });
            Graph.AddEdge(new Rel
            {
                Source = gf2,
                Name   = "父女",
                Target = mother
            });
            Graph.AddEdge(new Rel
            {
                Source = gf2,
                Name   = "父女",
                Target = Ayi
            });
            Graph.AddEdge(new Rel
            {
                Source = father,
                Target = mother,
                Name   = "夫妻"
            });
            Graph.AddEdge(new Rel
            {
                Source = father,
                Name   = "父子",
                Target = son
            });
            Graph.AddEdge(new Rel
            {
                Source = father,
                Name   = "父女",
                Target = daughter
            });
            
            
            Graph.AddEdge(new Rel
            {
                Source = gm1,
                Name   = "母子",
                Target = bobo
            });
            
            Graph.AddEdge(new Rel
            {
                Source = gm1,
                Name   = "母子",
                Target = father
            });
            
            Graph.AddEdge(new Rel
            {
                Source = gm1,
                Name   = "母子",
                Target = shushu
            });
            
            Graph.AddEdge(new Rel
            {
                Source = gm1,
                Name   = "母女",
                Target = gugu
            });
            
            Graph.AddEdge(new Rel
            {
                Source = gm2,
                Name   = "母女",
                Target = mother
            });
            
            Graph.AddEdge(new Rel
            {
                Source = gm2,
                Name   = "母女",
                Target = Ayi
            });
        }
        
        public PersonGraph Graph { get; }
    }
}