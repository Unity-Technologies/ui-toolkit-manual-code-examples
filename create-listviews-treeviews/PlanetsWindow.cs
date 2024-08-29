using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

// Base class for all windows that display planet information.
public class PlanetsWindow : EditorWindow
{
    [SerializeField]
    protected VisualTreeAsset uxmlAsset;

    // Nested interface that may be either a single planet or a group of planets.
    protected interface IPlanetOrGroup
    {
        public string name
        {
            get;
        }

        public bool populated
        {
            get;
        }
    }

    // Nested class that represents a planet.
    protected class Planet : IPlanetOrGroup
    {
        public string name
        {
            get;
        }

        public bool populated
        {
            get;
        }

        public Planet(string name, bool populated = false)
        {
            this.name = name;
            this.populated = populated;
        }
    }

    // Nested class that represents a group of planets.
    protected class PlanetGroup : IPlanetOrGroup
    {
        public string name
        {
            get;
        }

        public bool populated
        {
            get
            {
                var anyPlanetPopulated = false;
                foreach (Planet planet in planets)
                {
                    anyPlanetPopulated = anyPlanetPopulated || planet.populated;
                }
                return anyPlanetPopulated;
            }
        }

        public readonly IReadOnlyList<Planet> planets;

        public PlanetGroup(string name, IReadOnlyList<Planet> planets)
        {
            this.name = name;
            this.planets = planets;
        }
    }

    // Data about planets in our solar system.
    protected static readonly List<PlanetGroup> planetGroups = new List<PlanetGroup>
    {
        new PlanetGroup("Inner Planets", new List<Planet>
        {
            new Planet("Mercury"),
            new Planet("Venus"),
            new Planet("Earth", true),
            new Planet("Mars")
        }),
        new PlanetGroup("Outer Planets", new List<Planet>
        {
            new Planet("Jupiter"),
            new Planet("Saturn"),
            new Planet("Uranus"),
            new Planet("Neptune")
        })
    };

    // Expresses planet data as just a list of the planets themselves. Needed for ListView and MultiColumnListView.
    protected static List<Planet> planets
    {
        get
        {
            var retVal = new List<Planet>(8);
            foreach (var group in planetGroups)
            {
                retVal.AddRange(group.planets);
            }
            return retVal;
        }
    }

    // Expresses planet data as a list of TreeViewItemData objects. Needed for TreeView and MultiColumnTreeView.
    protected static IList<TreeViewItemData<IPlanetOrGroup>> treeRoots
    {
        get
        {
            int id = 0;
            var roots = new List<TreeViewItemData<IPlanetOrGroup>>(planetGroups.Count);
            foreach (var group in planetGroups)
            {
                var planetsInGroup = new List<TreeViewItemData<IPlanetOrGroup>>(group.planets.Count);
                foreach (var planet in group.planets)
                {
                    planetsInGroup.Add(new TreeViewItemData<IPlanetOrGroup>(id++, planet));
                }

                roots.Add(new TreeViewItemData<IPlanetOrGroup>(id++, group, planetsInGroup));
            }
            return roots;
        }
    }
}