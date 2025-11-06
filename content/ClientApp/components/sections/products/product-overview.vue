<template>
  <span>
    <!-- Section Products Start -->
    <div class="products">
      <div class="container">
        <div class="row products-wrapper">
          <div class="col-lg-3 products-slider__info">
            <h2>Reoler</h2>
            <p>Bogreoler, tv-reoler, dimsereoler. Shelfer kan det hele!</p>

            <div class="slider-controller">
              <div
                v-on:click="scroll('prev',0)"
                class="slider-controller__prev slider-controller__prev--bookshelf"
              >
                <img src="/resources/images/arrow.svg" alt="arrow icon" />
              </div>
              <div
                v-on:click="scroll('next',0)"
                class="slider-controller__next slider-controller__next--bookshelf"
              >
                <img src="/resources/images/arrow.svg" alt="arrow icon" />
              </div>
            </div>
          </div>
          <div class="col-lg-9">
            <div class="product-slider product-slider--bookshelf" id="productslider0">
              <div v-for="(bookcase,index) in bookcases" :key="index" class="product-slider__slide">
                <div v-on:click="seeMore('bookcase', index)" class="slide__image">
                  <img :src="bookcase.smallImage" alt="Shelfer product" />
                  <div v-if="bookcase.isNew" class="product__new">Nyhed</div>
                </div>
                <div class="slide__content">
                  <span>
                    Designet af
                    <b>{{bookcase.owner}}</b>
                  </span>
                  <button v-on:click="seeMore('bookcase', index)" class="btn">Se mere</button>
                  <!-- TODO: Include when function is available -->
                  <!-- <a href="#" class="btn">Benyt design</a> -->
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="row products-wrapper">
          <div class="col-lg-3 products-slider__info">
            <h2>Døråbningsreoler</h2>
            <p>Få en reol over din døråbning, og udnyt pladsen optimalt.</p>

            <div class="slider-controller">
              <div
                v-on:click="scroll('prev',1)"
                class="slider-controller__prev slider-controller__prev--bookshelf"
              >
                <img src="/resources/images/arrow.svg" alt="arrow icon" />
              </div>
              <div
                v-on:click="scroll('next',1)"
                class="slider-controller__next slider-controller__next--bookshelf"
              >
                <img src="/resources/images/arrow.svg" alt="arrow icon" />
              </div>
            </div>
          </div>
          <div class="col-lg-9">
            <div class="product-slider product-slider--bookshelf" id="productslider1">
              <div
                v-for="(bookcase,index) in doorBookcases"
                :key="index"
                class="product-slider__slide"
              >
                <div v-on:click="seeMore('doorBookcase', index)" class="slide__image">
                  <img :src="bookcase.smallImage" alt="Shelfer product" />
                  <div v-if="bookcase.isNew" class="product__new">Nyhed</div>
                </div>
                <div class="slide__content">
                  <span>
                    Designet af
                    <b>{{bookcase.owner}}</b>
                  </span>
                  <button v-on:click="seeMore('doorBookcase', index)" class="btn">Se mere</button>
                  <!-- TODO: Include when function is available -->
                  <!-- <a href="#" class="btn">Benyt design</a> -->
                </div>
              </div>
            </div>
          </div>
        </div>
        <div class="row products-wrapper">
          <div class="col-lg-3 products-slider__info">
            <h2>Andet</h2>
            <p>Shelfer kan bruges overalt i hjemmet. Du bestemmer selv, om dit møbel skal væghænges eller sættes på gulvet.</p>

            <div class="slider-controller">
              <div
                v-on:click="scroll('prev',2)"
                class="slider-controller__prev slider-controller__prev--bookshelf"
              >
                <img src="/resources/images/arrow.svg" alt="arrow icon" />
              </div>
              <div
                v-on:click="scroll('next',2)"
                class="slider-controller__next slider-controller__next--bookshelf"
              >
                <img src="/resources/images/arrow.svg" alt="arrow icon" />
              </div>
            </div>
          </div>
          <div class="col-lg-9">
            <div class="product-slider product-slider--bookshelf" id="productslider2">
              <div v-for="(bookcase,index) in other" :key="index" class="product-slider__slide">
                <div v-on:click="seeMore('other', index)" class="slide__image">
                  <img :src="bookcase.smallImage" alt="Shelfer product" />
                  <div v-if="bookcase.isNew" class="product__new">Nyhed</div>
                </div>
                <div class="slide__content">
                  <span>
                    Designet af
                    <b>{{bookcase.owner}}</b>
                  </span>
                  <button v-on:click="seeMore('other',index)" class="btn">Se mere</button>
                  <!-- TODO: Include when function is available -->
                  <!-- <a href="#" class="btn">Benyt design</a> -->
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
    <!-- Section Products End -->
    <see-more-modal v-if="seeMoreIndex !== null" :data="seeMoreData" @close="hideSeeMore"></see-more-modal>
  </span>
</template>

<script>
import seeMoreModal from "./see-more-modal";
export default {
  name: "product-overview",
  components: {
    "see-more-modal": seeMoreModal
  },
  data() {
    return {
      seeMoreIndex: null,
      seeMoreType: null,
      bookcases: [
        {
          title: "Nilas tv-reol",
          story:
            "Smukke reoler tilpasset efter vores behov. Efter at have fået diverse tilbud, som enten kostede spidsen af en jetjager eller bare ikke passede til vores behov fandt vi Shelfer. Hos Shelfer kunne vi få lige den løsning vi ønskede og Emil fra Shelfer var fantastisk til sparring omkring hvordan løsningen kunne udformes. Ikke nok med at vi fik rådgivning hele vejen igennem forløbet, kom de også ud og både målte op, samt monterede reolerne for os. Shelfer har ydet sublim service, smukke reoler og ikke mindst til fantastiske priser. Vi vil til enhver tid anbefale Shelfer og er allerede i dialog med dem omkring at vi gerne vil have lavet yderligere nogle reoler.",
          description: "",
          price:
            "16.419 kr. – grundpris\n3188 kr. – merpris, hvidmalede låger\n855 kr – fremrykket bagplade til tv-ophængning\n20.462 kr. i alt",
          location: "Vedbæk",
          owner: "Nila",
          smallImage: "/resources/images/products/nila1-small.jpg",
          largeImage: "/resources/images/products/nila1-large.jpg",
          isNew: false
        },
        {
          title: "Lizas helvægsreol",
          story:
            "Professionalism, customer service, and quality of work. Aced all of them! Highly recommend!!!",
          description: "",
          price: "16.572 kr.",
          location: "Kongens Lyngby",
          owner: "Liza",
          smallImage: "/resources/images/products/liza-small.jpg",
          largeImage: "/resources/images/products/liza-large.jpg",
          isNew: false
        },
        {
          title: "Klaes og Helens helvægsreol",
          story:
            "Vi har fået lavet en super fin reol af et par dygtige gutter som holder hvad de lover :D",
          description: "",
          price:
            "30.578 kr. – grundpris\n2000 kr. ekstra dybde i underskabe\n32.578 i alt",
          location: "Værløse",
          owner: "Klaes og Helene",
          smallImage: "/resources/images/products/klaes-small.jpg",
          largeImage: "/resources/images/products/klaes-large.jpg",
          isNew: false
        },
        {
          title: "Ines' tv-reol",
          story:
            "Super tilfreds både med hjemmesiden og levering. Er ret pjattet med selv at kunne designe reolen, der som færdigt resultat står virkelig flot. Rigtig god service. Vil klart anbefale Shelfer, hvis du vil forkæle dig selv med en lækker reol.",
          description: "",
          price: "9462 kr.",
          location: "Frederiksberg",
          owner: "Ines",
          smallImage: "/resources/images/products/ines-small.png",
          largeImage: "/resources/images/products/ines-large.png",
          isNew: false
        },
        {
          title: "Nilas nichereol",
          story:
            "Smukke reoler tilpasset efter vores behov. Efter at have fået diverse tilbud, som enten kostede spidsen af en jetjager eller bare ikke passede til vores behov fandt vi Shelfer. Hos Shelfer kunne vi få lige den løsning vi ønskede og Emil fra Shelfer var fantastisk til sparring omkring hvordan løsningen kunne udformes. Ikke nok med at vi fik rådgivning hele vejen igennem forløbet, kom de også ud og både målte op, samt monterede reolerne for os. Shelfer har ydet sublim service, smukke reoler og ikke mindst til fantastiske priser. Vi vil til enhver tid anbefale Shelfer og er allerede i dialog med dem omkring at vi gerne vil have lavet yderligere nogle reoler.",
          description: "",
          price: "9006 kr.",
          location: "Vedbæk",
          owner: "Nila",
          smallImage: "/resources/images/products/nila2-small.jpg",
          largeImage: "/resources/images/products/nila2-large.jpg",
          isNew: false
        }
      ],
      doorBookcases: [
        {
          title: "Sarahs reol",
          story:
            "Lørdagen bruges på at dimse og indrette vores nye, kæmpestore vægreol🙌 Jeg kan allerede fornemme at den bliver en ny favorit herhjemme. Også selvom det kun er mig som ejer 5% af de bøger vi har herhjemme. Så er det jo pænt😍 Tak til @shelfer_furniture for god service og en fabelagtig reol. Vi er så glade for den!",
          description: "",
          price:
            "13.398 kr. – grundpris\n1339 kr. – merpris, særdesign med to døråbninger\n500 kr. – udskæring til ledning\n15.237 kr. – i alt",
          location: "Brønshøj",
          owner: "Sarah",
          smallImage: "/resources/images/products/sarah-small.jpg",
          largeImage: "/resources/images/products/sarah-large.jpg",
          isNew: false
        },
        {
          title: "Andreas og Maikens reol",
          story: "",
          description: "",
          price: "9348 kr.",
          location: "København N",
          owner: "Andreas og Maiken",
          smallImage: "/resources/images/products/andreas-small.jpg",
          largeImage: "/resources/images/products/andreas-large.jpg",
          isNew: false
        }
      ],
      other: [
        {
          title: "Nynnes tv-møbel",
          story:
            "Købte en reol hos Shelfer, gik selv ind og designede den på deres hjemmeside og havde mulighed for at tilføje enkelt ændring uden problemer (væghængt). God kommunikation på mails og hurtige svar.",
          description: "",
          price: "5157 kr.",
          location: "Vanløse",
          owner: "Nynne",
          smallImage: "/resources/images/products/nynne-small.jpg",
          largeImage: "/resources/images/products/nynne-large.jpg",
          isNew: false
        },
        {
          title: "Anders' entrereol",
          story:
            "Fremragende rådgivning og det færdige resultat er blevet så fedt. Kan kun anbefale shelfer og det er ikk sidste gang vi handler der.",
          description: "",
          price: "4783 kr.",
          location: "København S",
          owner: "Anders",
          smallImage: "/resources/images/products/anders-small.jpg",
          largeImage: "/resources/images/products/anders-large.jpg",
          isNew: false
        }
      ]
    };
  },
  methods: {
    seeMore: function(type, index) {
      this.seeMoreType = type;
      this.seeMoreIndex = index;
    },
    hideSeeMore: function() {
      this.seeMoreIndex = null;
      this.seeMoreType = null;
    },
    scroll: function(direction, element) {
      var elt = document.querySelector("#productslider" + element);
      var scrollIndex = Math.max(Math.floor(elt.scrollLeft / 370), 0);
      var skipExtraPrev = elt.scrollLeft - scrollIndex * 370 < 20;
      var maxScroll = elt.scrollWidth - elt.clientWidth;
      if (direction === "prev") {
        elt.scrollLeft = Math.max(
          (scrollIndex - (skipExtraPrev ? 1 : 0)) * 370,
          0
        );
      } else {
        elt.scrollLeft = Math.min((scrollIndex + 1) * 370, maxScroll);
      }
    }
  },
  computed: {
    seeMoreData: function() {
      if (this.seeMoreType === "bookcase") {
        return this.bookcases[this.seeMoreIndex];
      }
      if (this.seeMoreType === "other") {
        return this.other[this.seeMoreIndex];
      }
      if (this.seeMoreType === "doorBookcase") {
        return this.doorBookcases[this.seeMoreIndex];
      }
    }
  }
};
</script>

<style>
/*--------------------------------------------------------------
# Products styling
--------------------------------------------------------------*/
.products {
  margin-top: 70px;
}

.products-wrapper {
  margin-bottom: 75px;
}

.products-slider__info {
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-orient: vertical;
  -webkit-box-direction: normal;
  -ms-flex-direction: column;
  flex-direction: column;
}
.products-slider__info .slider-controller {
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -ms-flex-item-align: end;
  align-self: flex-end;
  margin: 15px 0;
}
@media only screen and (min-width: 992px) {
  .products-slider__info .slider-controller {
    margin: auto -15px 92px 0;
  }
}
.products-slider__info .slider-controller .slider-controller__prev,
.products-slider__info .slider-controller .slider-controller__next {
  -webkit-box-flex: 1;
  -ms-flex: 1 0 50%;
  flex: 1 0 50%;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -webkit-box-pack: center;
  -ms-flex-pack: center;
  justify-content: center;
  -webkit-box-align: center;
  -ms-flex-align: center;
  align-items: center;
  border: 1px solid #d8d8d8;
  cursor: pointer;
}
.products-slider__info .slider-controller .slider-controller__prev img,
.products-slider__info .slider-controller .slider-controller__next img {
  padding: 18px 20px;
}
.products-slider__info .slider-controller .slider-controller__next {
  border-left: none;
}
.products-slider__info .slider-controller .slider-controller__next img {
  -webkit-transform: rotate(180deg);
  -ms-transform: rotate(180deg);
  transform: rotate(180deg);
}

.product-slider {
  display: flex;

  overflow-x: auto;
  scroll-snap-type: x mandatory;

  scroll-behavior: smooth;
  -webkit-overflow-scrolling: touch;
}
.product-slider .slick-list {
  margin-right: -20px;
}
.product-slider__slide {
  scroll-snap-align: start;
  flex-shrink: 0;
  max-width: 350px;
  margin-right: 20px;
}
.product-slider__slide .slide__image {
  position: relative;
  height: 350px;
  overflow: hidden;
}
.product-slider__slide .slide__image img {
  min-height: 100%;
  min-width: 100%;
  -o-object-fit: cover;
  object-fit: cover;
}
.product-slider__slide .slide__image .product__new {
  position: absolute;
  top: 10px;
  right: 10px;
  padding: 10px 30px;
  background-color: #ffffff;
  color: #2dac86;
  font-weight: 800;
  text-transform: uppercase;
}
.product-slider__slide .slide__content {
  margin-bottom: 10px;
}
.product-slider__slide .slide__content span {
  display: block;
  font-size: 12px;
  line-height: 18px;
  color: #4e4e4e;
  margin-top: 10px;
  margin-bottom: 15px;
}
</style>
