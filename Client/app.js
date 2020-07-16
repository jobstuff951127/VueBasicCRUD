Vue.prototype.$httpRequest = axios;
// axios.interceptors.response.use(
//   (response) => {
//     if (response.status === 200) {
//       alert("You are not authorized");
//     }
//     return response;
//   },
//   (error) => {
//     if (error.response && error.response.data) {
//       return Promise.reject(error.response.data);
//     }
//     return Promise.reject(error.message);
//   }
// );
new Vue({
  el: "#app",
  data: {
    url: "http://localhost:5000/api/ContactosMavidavidBlancos/",
    secs: 3000,
    contacts: [],
    loader: false,
    CenteringStyle: {
      width: "1200px",
      height: "700px",
      display: "flex",
      justifyContent: "center",
      alignItems: "center",
    },
    Name: "",
    form: {
      id: 0,
      Name: "",
      TipoContacto: "",
      TelefonoFijo: 0,
      TelefonoCelular: 0,
      FechaNacimiento: "",
      Sexo: "",
      EstadoCivil: "",
    },
    selectContact: ["Familiar", "Amistad", "Otros"],
    selectCivilStatus: ["Casado(a)", "Soltero(a)", "Union Libre"],
    selectSex: ["Masculino", "Femenino"],
  },
  methods: {
    GetContacts: async function () {
      await this.$httpRequest
        .get(this.url)
        .then((response) => {
          this.contacts = response.data;
          console.log(response.data);
        })
        .catch((error) => {
          iziToast.show({
            icon: "fa fa-exclamation-triangle",
            title: "Error",
            color: "red",
            progressBar: false,
            timeout: false,
            balloon: true,
            animateInside: true,
            drag: true,
            message: error.toString(),
          });
        })
        .finally(() => (this.loader = true));
    },
    GetByLike: async function () {
      await this.$httpRequest
        .get(this.url + "GetContactsByName" + "/" + this.Name)
        .then((response) => {
          this.contacts = [];
          this.contacts = response.data;
        })
        .catch((error) => {
          //this.contacts = [];
          this.GetContacts();
          // iziToast.show({
          //   icon: "fa fa-exclamation-triangle",
          //   title: "Error",
          //   color: "red",
          //   //progressBar: false,
          //   //timeout: false,
          //   balloon: true,
          //   animateInside: true,
          //   drag: true,
          //   message: error.toString(),
          // });
        });
    },
    GetOne: async function (id) {
      console.log(id);
      await this.$httpRequest
        .get(this.url + id)
        .then((response) => {
          this.form.id = response.data.id;
          this.form.Name = response.data.nombre;
          this.form.EstadoCivil = response.data.estadoCivil;
          this.form.Sexo = response.data.sexo;
          this.form.TelefonoCelular = response.data.telefonoMovil;
          this.form.TelefonoFijo = response.data.telefonoFijo;
          this.form.TipoContacto = response.data.tipoContacto;
          this.form.FechaNacimiento = response.data.fecheDeNacimiento;
        })
        .catch((error) => {
          iziToast.show({
            icon: "fa fa-exclamation-triangle",
            title: "Error",
            color: "red",
            progressBar: false,
            timeout: false,
            balloon: true,
            animateInside: true,
            drag: true,
            message: error.toString(),
          });
        });
    },
    DeleteOne: async function (id) {
      await this.$httpRequest
        .delete(this.url + id)
        .then((response) => {
          iziToast.success({
            //icon: "fa fa-exclamation-triangle",
            title: "success",
            progressBar: false,
            balloon: true,
            animateInside: true,
            drag: true,
            message: "Contact Deleted",
          });
          this.GetContacts();
        })
        .catch((error) => {
          iziToast.show({
            icon: "fa fa-exclamation-triangle",
            title: "Error",
            color: "red",
            progressBar: false,
            timeout: false,
            balloon: true,
            animateInside: true,
            drag: true,
            message: error.toString(),
          });
        });
    },
    Insert: async function () {
      await this.$httpRequest
        .post(this.url, {
          Nombre: this.form.Name,
          TipoContacto: this.form.TipoContacto,
          TelefonoFijo: this.form.TelefonoFijo.toString(),
          TelefonoMovil: this.form.TelefonoCelular.toString(),
          FecheDeNacimiento: this.form.FechaNacimiento,
          Sexo: this.form.Sexo,
          EstadoCivil: this.form.EstadoCivil,
        })
        .then((response) => {
          iziToast.success({
            //icon: "fa fa-exclamation-triangle",
            title: "success",
            progressBar: false,
            balloon: true,
            animateInside: true,
            drag: true,
            message: "Contact Inserted",
          });
          this.GetContacts();
          $("#exampleModalCenter2").modal("hide");
        })
        .catch((error) => {
          iziToast.show({
            icon: "fa fa-exclamation-triangle",
            title: "Error",
            color: "red",
            progressBar: false,
            timeout: false,
            balloon: true,
            animateInside: true,
            drag: true,
            message: error.toString(),
          });
        });
    },
    Update: async function () {
      console.log(this.url + this.form.id);
      await this.$httpRequest
        .put(this.url + this.form.id, {
          Nombre: this.form.Name,
          TipoContacto: this.form.TipoContacto,
          TelefonoFijo: this.form.TelefonoFijo.toString(),
          TelefonoMovil: this.form.TelefonoCelular.toString(),
          FecheDeNacimiento: this.form.FechaNacimiento,
          Sexo: this.form.Sexo,
          EstadoCivil: this.form.EstadoCivil,
        })
        .then((response) => {
          iziToast.success({
            icon: "fa fa-exclamation-triangle",
            title: "success",
            progressBar: false,
            balloon: true,
            animateInside: true,
            drag: true,
            message: "Contact Updated",
          });
          this.GetContacts();
          $("#exampleModalCenter").modal("hide");
        })
        .catch((error) => {
          iziToast.show({
            icon: "fa fa-exclamation-triangle",
            title: "Error",
            color: "red",
            progressBar: false,
            timeout: false,
            balloon: true,
            animateInside: true,
            drag: true,
            message: error.toString(),
          });
        });
    },

    SetForm: function () {
      (this.form.Name = ""),
        (this.form.TipoContacto = ""),
        (this.form.TelefonoFijo = ""),
        (this.form.TelefonoCelular = ""),
        (this.form.FechaNacimiento = ""),
        (this.form.Sexo = ""),
        (this.form.EstadoCivil = "");
    },
  },
  mounted() {
    setTimeout(() => {
      this.GetContacts();
    }, this.secs);
  },
});
