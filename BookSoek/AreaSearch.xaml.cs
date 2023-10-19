using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BookSoek
{
    /// <summary>
    /// Interaction logic for AreaSearch.xaml
    /// </summary>
    /// 

    
    public partial class AreaSearch : Page
    {

        private static Dictionary<string, string> callNumberDict = new Dictionary<string, string>()
        {
            {"010", "Bibliographies"},
            {"020", "Library and information sciences"},
            {"030", "Encyclopaedias and books of facts"},
            {"040", "Unassigned (formerly Biographies)"},
            {"050", "Magazines, journals, and serials"},
            {"060", "Associations, organizations, and museums"},
            {"070", "News media, journalism, and publishing"},
            {"080", "Quotations"},
            {"090", "Manuscripts and rare books"},
            {"100", "Philosophy"},
            {"110", "Metaphysics"},
            {"120", "Epistemology"},
            {"130", "Parapsychology and occultism"},
            {"140", "Philosophical schools of thought"},
            {"150", "Psychology"},
            {"160", "Philosophical logic"},
            {"170", "Ethics"},
            {"180", "Ancient, medieval, and Eastern philosophy"},
            {"190", "Modern Western philosophy (19th-century, 20th-century)"},
            {"200", "Religion"},
            {"210", "Philosophy and theory of religion"},
            {"220", "The Bible"},
            {"230", "Christianity"},
            {"240", "Christian practice and observance"},
            {"250", "Christian orders and local church"},
            {"260", "Social and ecclesiastical theology"},
            {"270", "History of Christianity"},
            {"280", "Christian denominations"},
            {"290", "Other religions"},
            {"300", "Social sciences, sociology, and anthropology"},
            {"310", "Statistics"},
            {"320", "Political science"},
            {"330", "Economics"},
            {"340", "Law"},
            {"350", "Public administration and military science"},
            {"360", "Social problems and social services"},
            {"370", "Education"},
            {"380", "Commerce, communications, and transportation"},
            {"390", "Customs, etiquette, and folklore"},
            {"400", "Language"},
            {"410", "Linguistics"},
            {"420", "English and Old English languages"},
            {"430", "German and related languages"},
            {"440", "French and related languages"},
            {"450", "Italian, Romanian, and related languages"},
            {"460", "Spanish, Portuguese, Galician"},
            {"470", "Latin and Italic languages"},
            {"480", "Classical and modern Greek languages"},
            {"490", "Other languages"},
            {"500", "Science"},
            {"510", "Mathematics"},
            {"520", "Astronomy"},
            {"530", "Physics"},
            {"540", "Chemistry"},
            {"550", "Earth sciences and geology"},
            {"560", "Paleontology"},
            {"570", "Biology"},
            {"580", "Plants"},
            {"590", "Animals"},
            {"600", "Technology"},
            {"610", "Medicine and health"},
            {"620", "Engineering"},
            {"630", "Agriculture"},
            {"640", "Home and family management"},
            {"650", "Management and public relations"},
            {"660", "Chemical engineering"},
            {"670", "Manufacturing"},
            {"680", "Manufacture for specific uses"},
            {"690", "Construction of buildings"},
            {"700", "The Arts"},
            {"710", "Area planning and landscape architecture"},
            {"720", "Architecture"},
            {"730", "Sculpture, ceramics, and metalwork"},
            {"740", "Graphic arts and decorative arts"},
            {"750", "Painting"},
            {"760", "Printmaking and prints"},
            {"770", "Photography, computer art, film, video"},
            {"780", "Music"},
            {"790", "Recreational and performing arts"},
            {"800", "Literature"},
            {"810", "American literature in English"},
            {"820", "English and Old English literatures"},
            {"830", "German and related literatures"},
            {"840", "French and related literatures"},
            {"850", "Italian, Romanian, and related literatures"},
            {"860", "Spanish, Portuguese, Galician literatures"},
            {"870", "Latin and Italic literatures"},
            {"880", "Classical and modern Greek literatures"},
            {"890", "Other literatures"},
            {"900", "History"},
            {"910", "Geography and travel"},
            {"920", "Biography and genealogy"},
            {"930", "History of the ancient world (to c. 499)"},
            {"940", "History of Europe"},
            {"950", "History of Asia"},
            {"960", "History of Africa"},
            {"970", "History of North America"},
            {"980", "History of South America"},
            {"990", "History of other areas"}
        };


        public AreaSearch()
        {
            InitializeComponent();
        }
    }
}
