import { library } from "@fortawesome/fontawesome-svg-core";
// Official documentation available at: https://github.com/FortAwesome/vue-fontawesome
import { FontAwesomeIcon } from "@fortawesome/vue-fontawesome";

// If not present, it won't be visible within the application. Considering that you
// don't want all the icons for no reason. This is a good way to avoid importing too many
// unnecessary things.
library.add(
  require("@fortawesome/free-solid-svg-icons").faEnvelope,
  require("@fortawesome/free-solid-svg-icons").faHeart,
  require("@fortawesome/free-solid-svg-icons").faGraduationCap,
  require("@fortawesome/free-solid-svg-icons").faHome,
  require("@fortawesome/free-solid-svg-icons").faInfo,
  require("@fortawesome/free-solid-svg-icons").faList,
  require("@fortawesome/free-solid-svg-icons").faSpinner,
  require("@fortawesome/free-solid-svg-icons").faUser,
  require("@fortawesome/free-solid-svg-icons").faPen,
  require("@fortawesome/free-solid-svg-icons").faIdCard,
  require("@fortawesome/free-solid-svg-icons").faUsers,
  require("@fortawesome/free-solid-svg-icons").faSignInAlt,
  require("@fortawesome/free-solid-svg-icons").faSignOutAlt,
  require("@fortawesome/free-solid-svg-icons").faUserCheck,
  require("@fortawesome/free-solid-svg-icons").faBars,
  require("@fortawesome/free-solid-svg-icons").faSave,
  require("@fortawesome/free-solid-svg-icons").faTimes,
  require("@fortawesome/free-solid-svg-icons").faTimesCircle,
  require("@fortawesome/free-solid-svg-icons").faChevronUp,
  require("@fortawesome/free-solid-svg-icons").faChevronDown,
  require("@fortawesome/free-solid-svg-icons").faTrashAlt,
  require("@fortawesome/free-solid-svg-icons").faShoppingCart,
  require("@fortawesome/free-solid-svg-icons").faCashRegister,
  require("@fortawesome/free-solid-svg-icons").faPlus,
  require("@fortawesome/free-solid-svg-icons").faMinus,
  require("@fortawesome/free-solid-svg-icons").faCheck,
  require("@fortawesome/free-solid-svg-icons").faBorderAll,
  require("@fortawesome/free-solid-svg-icons").faTruck,
  require("@fortawesome/free-solid-svg-icons").faArrowsAltV,
  require("@fortawesome/free-solid-svg-icons").faArrowsAltH,
  require("@fortawesome/free-solid-svg-icons").faArrowLeft,
  require("@fortawesome/free-solid-svg-icons").faArrowRight,
  require("@fortawesome/free-solid-svg-icons").faArrowLeft,
  require("@fortawesome/free-solid-svg-icons").faArrowRight,
  require("@fortawesome/free-solid-svg-icons").faUndo,
  require("@fortawesome/free-solid-svg-icons").faRedo,
  require("@fortawesome/free-solid-svg-icons").faQuestion,
  require("@fortawesome/free-solid-svg-icons").faQuestionCircle,
  require("@fortawesome/free-solid-svg-icons").faCog,
  require("@fortawesome/free-solid-svg-icons").faQuoteLeft,
  require("@fortawesome/free-solid-svg-icons").faQuoteRight
);

export { FontAwesomeIcon };
