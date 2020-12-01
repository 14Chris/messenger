<template>
  <div class="container">
    <h4>Register</h4>
    <form @submit.prevent="register">
      <!-- First name -->
      <b-field label="First name" v-if="!$v.model.first_name.$invalid">
        <b-input type="text" v-model="model.first_name" autofocus></b-input>
      </b-field>

      <b-field v-else label="First name" type="is-danger" message="First name is required">
        <b-input type="text" v-model="model.first_name" autofocus></b-input>
      </b-field>

      <!-- Last name -->
      <b-field label="Last name" v-if="!$v.model.last_name.$invalid">
        <b-input type="text" v-model="model.last_name"></b-input>
      </b-field>

      <b-field v-else label="Last name" type="is-danger" message="Last name is required">
        <b-input type="text" v-model="model.last_name"></b-input>
      </b-field>

      <!-- Email -->
      <b-field label="Email" v-if="!$v.model.email.$invalid">
        <b-input type="email" v-model="model.email"></b-input>
      </b-field>

      <b-field v-else label="Email" type="is-danger" :message="GetEmailErrors()">
        <b-input type="email" v-model="model.email"></b-input>
      </b-field>

      <!-- Password -->
       <b-field label="Passworrd" v-if="!$v.model.password.$invalid">
        <b-input type="password" v-model="model.password" password-reveal></b-input>
      </b-field>

      <b-field v-else label="Password" type="is-danger" :message="GetPasswordErrors()">
        <b-input type="password" v-model="model.password" password-reveal></b-input>
      </b-field>

      <!-- Password confirmation -->
       <b-field label="Confirm Password" v-if="!$v.model.password_confirmation.$invalid">
        <b-input type="password" v-model="model.password_confirmation" password-reveal></b-input>
      </b-field>

      <b-field v-else label="Password" type="is-danger" :message="GetConfirmPasswordErrors()">
        <b-input type="password" v-model="model.password_confirmation" password-reveal></b-input>
      </b-field>

      <div>
        <b-button native-type="submit">Register</b-button>
      </div>
      <p>Already have an account ? Log in :</p>
      <b-button tag="router-link" to="/login" type="is-info">Sign in</b-button>
    </form>
  </div>
</template>

<script>
import { required, minLength, sameAs, email } from "vuelidate/lib/validators";
import ApiService from "../service/api";

var api = new ApiService();

export default {
  name: "Register",
  data() {
    return {
      model: {
        first_name: "",
        last_name: "",
        email: "",
        password: "",
        password_confirmation: "",
      },
    };
  },
  methods: {
    register: function () {
      let data = {
        first_name: this.model.first_name,
        last_name: this.model.last_name,
        email: this.model.email,
        password: this.model.password,
      };
      this.$v.$touch();
      if (this.$v.$invalid) {
        console.log("error register");
      } else {
        api
          .create("users", JSON.stringify(data))
          .then((response) => response.json()).then(data=>{
            if (data.HttpStatus == 201) {
              this.$buefy.notification.open({
                message: "Your account has been created. You can now log in",
                type: "is-success",
                duration: 5000,
                closable: true,
              });
              this.$router.push("/login");
            } else {
              this.$buefy.notification.open({
                message: "An error happened while creating your account",
                type: "is-danger",
                duration: 5000,
                closable: true,
              });
            }
          })
          .catch(() => {
            this.$buefy.notification.open({
              message: "An error happened while creating your account",
              type: "is-danger",
              duration: 5000,
              closable: true,
            });
          });
      }
    },
    GetEmailErrors() {
      var errors = [];
      if (!this.$v.model.email.required) {
        errors.push("Email is required");
      } else {
        if (!this.$v.model.email.email) {
          errors.push("Please enter valid email address");
        }
        if (!this.$v.model.email.isUnique) {
          errors.push("Email is already taken");
        }
      }
      return errors;
    },
    GetPasswordErrors() {
      var errors = [];
      if (!this.$v.model.password.required) {
        errors.push("Password is required");
      } else {
        if (!this.$v.model.password.minLength) {
          errors.push(
            "Password must have at least " +
              this.$v.model.password.$params.minLength.min +
              " letters."
          );
        }
        if (!this.$v.model.password.oneNumber) {
          errors.push("Password must have at least one number");
        }
        if (!this.$v.model.password.oneUpperCase) {
          errors.push("Password must have at least one upper case character");
        }
        if (!this.$v.model.password.oneLowerCase) {
          errors.push("Password must have at least one lower case character");
        }
      }
      return errors;
    },
    GetConfirmPasswordErrors() {
      var errors = [];
      if (!this.$v.model.password_confirmation.required) {
        errors.push("Confirmation password is required");
      } else if (!this.$v.model.password_confirmation.sameAsPassword) {
        errors.push("Confirmation password has to be the same as password");
      }
      return errors;
    },
  },
  validations: {
    model: {
      first_name: {
        required,
      },
      last_name: {
        required,
      },
      email: {
        required,
        email,
        async isUnique(value) {
          if (value == null || value == "") return true;

          if (this.chkUsernameAvailabilityTimer) {
            clearTimeout(this.chkUsernameAvailabilityTimer);
            this.chkUsernameAvailabilityTimer = null;
          }
          return new Promise((resolve) => {
            this.chkUsernameAvailabilityTimer = setTimeout(() => {
              api.getData("users/email_exists/" + value).then((response) => response.json()).then(data=>{
                console.log(data.HttpStatus);
                if (data.HttpStatus == 200) {
                  resolve(true);
                } else {
                  resolve(false);
                }
              })
            }, 500);
          });
        },
      },
      password: {
        required,
        minLength: minLength(8),
        oneNumber(password) {
          return /(?=.*\d)/.test(password);
        },
        oneUpperCase(password) {
          return /(?=.*[A-Z])/.test(password);
        },
        oneLowerCase(password) {
          return /(?=.*[a-z])/.test(password);
        },
      },
      password_confirmation: {
        required,
        sameAsPassword: sameAs("password"),
      },
    },
  },
};
</script>

<style>
.error {
  color: red;
}
</style>