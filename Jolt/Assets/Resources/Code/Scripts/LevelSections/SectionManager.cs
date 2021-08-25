using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Jolt
{
    namespace LevelSections
    {
        public class SectionManager : MonoSingleton<SectionManager>, ISectionManager
        {
            [SerializeField] private GameObject _sectionsHolder;

            private List<ISection> _sections = new List<ISection>();

            public List<ISection> Sections { get => _sections; }
            public ISection CurrentSection { get; set; }

            public void OnPlayerTransitionedSection(string fromId, string toId)
            {
                /*
                  Gets exit section via id and runs its Exit function.
                  Gets entering section via id and runs its Enter function.
                  Transitions camera to new section.
                  Sets new respawn point for player.
                  Can even lower the games volume while transitioning or things like that.
                 */

                ISection fromSection = _sections.First(s => s.ID == fromId);
                ISection toSection = _sections.First(s => s.ID == toId);

                CurrentSection = fromSection;

                toSection.Enter();
                fromSection.Exit();

                //SectionCameraAdjuster.Current.Transition(toSection, fromSection);
            }

            // Start is called before the first frame update
            void Start()
            {
                GetSections();
                CurrentSection = Sections[0];
                CurrentSection.Enter();
            }

            public override void Init()
            {
                base.Init();

                ISectionTransitionController.OnPlayerTransitionedSection += OnPlayerTransitionedSection;
            }

            private void GetSections()
            {
                foreach(ISection section in _sectionsHolder.GetComponentsInChildren<ISection>())
                {
                    _sections.Add(section);
                }
            }
        }
    }
}


