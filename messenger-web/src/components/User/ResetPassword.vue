<template>
  <div class="container">
    <h3 class="title is-3">Reset password</h3>
    <div v-if="tokenOk == null">Loading...</div>
    <div v-else-if="tokenOk == true">
      token : {{ model.token }}
      <form @submit.prevent="ResetPassword">
        <!-- New password -->
        <div class="field" v-if="!$v.model.newPassword.$invalid">
          <label class="label">New password</label>
          <div class="control">
            <input
              class="input"
              type="password"
              placeholder="New password"
              v-model="model.newPassword"
            />
          </div>
        </div>

        <div class="field" v-else>
          <label class="label">New password</label>
          <div class="control">
            <input
              class="input is-danger"
              type="password"
              placeholder="New password"
              v-model="model.newPassword"
            />
          </div>
          <p class="help is-danger">{{ GetPasswordErrors() }}</p>
        </div>

        <!-- New password confirmation-->
        <div class="field" v-if="!$v.model.repeatNewPassword.$invalid">
          <label class="label">Confirm new password</label>
          <div class="control">
            <input
              class="input"
              type="password"
              placeholder="Confirm new password"
              v-model="model.repeatNewPassword"
            />
          </div>
        </div>

        <div class="field" v-else>
          <label class="label">Confirm new password</label>
          <div class="control">
            <input
              class="input is-danger"
              type="password"
              placeholder="Confirm new password"
              v-model="model.repeatNewPassword"
            />
          </div>
          <p class="help is-danger">{{ GetConfirmPasswordErrors() }}</p>
        </div>

        <button class="button is-primary">Change</button>
      </form>
    </div>
    <div v-else>Error during token validation</div>
  </div>
</template>

<script>
import { required, minLength, sameAs } from "vuelidate/lib/validators";
import ApiService from "@/service/api";

var api = new ApiService();
export default {
  data() {
    return {
      tokenOk: null,
      model: {
        newPassword: "",
        repeatNewPassword: "",
        token: "",
      },
    };
  },
  mounted() {
    this.model.token = this.$route.params.token;
    this.ValidateToken(this.model.token);
  },
  methods: {
    ValidateToken(token) {
      api
        .getData("users/reset_password/" + token)
        .then((response) => {
          console.log(response);
          if (response.ok == true) {
            response.json().then((data) => {
              if (data.ResponseType == 1) {
                this.tokenOk = true;
              } else {
                this.tokenOk = false;
                this.$buefy.notification.open({
                  message: "Invalid token",
                  type: "is-success",
                  duration: 5000,
                  closable: true,
                });
                this.$router.push("/login");
              }
            });
          }
        })
        .catch(() => {
          this.tokenOk = false;
          this.$buefy.notification.open({
            message: "Invalid token",
            type: "is-success",
            duration: 5000,
            closable: true,
          });
          this.$router.push("/login");
        });
    },
    ResetPassword() {
      var resetPasswordModel = {
        newPassword: this.model.newPassword,
        token: this.model.token,
      };

      api
        .create("users/reset_password", JSON.stringify(resetPasswordModel))
        .then((response) => {
          console.log(response);
          if (response.ok == true) {
            response.json().then((data) => {
              if (data.ResponseType == 1) {
                this.$buefy.notification.open({
                  message:
                    "Your password have been changed. You can now log in.",
                  type: "is-success",
                  duration: 5000,
                  closable: true,
                });
                this.$router.push("/login");
              } else {

                var errorMessage = ""

                  switch(data.Message){
                      case "SAME_NEW_PASSWORD":errorMessage = "New password has to be different from old password"
                      break;
                      case "NEW_PASSWORD_TOO_WEAK":errorMessage = "New password is too weak"
                      break;
                      default:errorMessage = "Error while changing your password"
                  }

                  this.$buefy.notification.open({
                    message: errorMessage,
                    type: "is-danger",
                    duration: 5000,
                    closable: true,
                  })
              }
            });
          }
        })
        .catch(() => {
          this.$buefy.notification.open({
            message: "Error while resetting your password",
            type: "is-danger",
            duration: 5000,
            closable: true,
          });
        });
    },
    GetPasswordErrors() {
      var errors = [];
      if (!this.$v.model.newPassword.required) {
        errors.push("New password is required");
      } else {
        if (!this.$v.model.newPassword.minLength) {
          errors.push(
            "New password must have at least " +
              this.$v.model.newPassword.$params.minLength.min +
              " letters."
          );
        }
        if (!this.$v.model.newPassword.oneNumber) {
          errors.push("New password must have at least one number");
        }
        if (!this.$v.model.newPassword.oneUpperCase) {
          errors.push(
            "New password must have at least one upper case character"
          );
        }
        if (!this.$v.model.newPassword.oneLowerCase) {
          errors.push(
            "New password must have at least one lower case character"
          );
        }
      }
      return errors;
    },
    GetConfirmPasswordErrors() {
      var errors = [];
      if (!this.$v.model.repeatNewPassword.required) {
        errors.push("Confirmation password is required");
      } else if (!this.$v.model.repeatNewPassword.sameAsPassword) {
        errors.push("Confirmation password has to be the same as password");
      }
      return errors;
    },
  },
  validations: {
    model: {
      newPassword: {
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
      repeatNewPassword: {
        required,
        sameAsPassword: sameAs("newPassword"),
      },
    },
  },
};
</script>

<style>
</style>