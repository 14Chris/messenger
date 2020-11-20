<template>
  <div>
      <h5 class="title is-5">Change password</h5>
      <form @submit.prevent="ChangePassword">
          <!-- Old password -->
          <div class="field" v-if="!$v.model.oldPassword.$invalid">
            <label class="label">Old password</label>
            <div class="control">
                <input class="input" type="password" placeholder="Old password" v-model="model.oldPassword">
            </div>
        </div>

        <div class="field" v-else>
        <label class="label">Old password</label>
        <div class="control">
            <input class="input is-danger" type="password" placeholder="Old password" v-model="model.oldPassword">
        </div>
        <p class="help is-danger">Old password is required</p>
        </div>

      <div class="field" v-if="!$v.model.newPassword.$invalid">
            <label class="label">New password</label>
            <div class="control">
                <input class="input" type="password" placeholder="New password" v-model="model.newPassword">
            </div>
        </div>

        <div class="field" v-else>
        <label class="label">New password</label>
        <div class="control">
            <input class="input is-danger" type="password" placeholder="New password" v-model="model.newPassword">
        </div>
        <p class="help is-danger">{{GetPasswordErrors()}}</p>
        </div>

       <div class="field" v-if="!$v.model.repeatPassword.$invalid">
            <label class="label">Confirm new password</label>
            <div class="control">
                <input class="input" type="password" placeholder="Confirm new password" v-model="model.repeatPassword">
            </div>
        </div>

        <div class="field" v-else>
        <label class="label">Confirm new password</label>
        <div class="control">
            <input class="input is-danger" type="password" placeholder="Confirm new password" v-model="model.repeatPassword">
        </div>
        <p class="help is-danger">{{GetConfirmPasswordErrors()}}</p>
        </div>

       <button class="button is-primary">Change</button>
      </form>
  </div>
</template>

<script>
import { required, minLength, sameAs, } from "vuelidate/lib/validators";
import ApiService from "@/service/api";

var api = new ApiService();
export default {
    data(){
        return {
            model:{
                oldPassword:"",
                newPassword:"",
                repeatPassword:""
            }
        }
    },
    methods:{
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
          errors.push("New password must have at least one upper case character");
        }
        if (!this.$v.model.newPassword.oneLowerCase) {
          errors.push("New password must have at least one lower case character");
        }
      }
      return errors;
    },
    GetConfirmPasswordErrors() {
      var errors = [];
      if (!this.$v.model.repeatPassword.required) {
        errors.push("Confirmation password is required");
      } else if (!this.$v.model.repeatPassword.sameAsPassword) {
        errors.push("Confirmation password has to be the same as password");
      }
      return errors;
    },
        ChangePassword(){
            if (!this.$v.$invalid) {
        api
          .update("users/password", JSON.stringify(this.model))
          .then((response) => {
            console.log(response);
            if (response.ok == true) {
              response.json().then((data) => {
                if (data.ResponseType == 1) {
                  this.$buefy.notification.open({
                    message: "Password have been changed",
                    type: "is-success",
                    duration: 5000,
                    closable: true,
                  });
                } else {
                  this.$buefy.notification.open({
                    message: "Error while changing password",
                    type: "is-danger",
                    duration: 5000,
                    closable: true,
                  });
                }
              });
            } else {
              this.$buefy.notification.open({
                message: "Error while changing password",
                type: "is-danger",
                duration: 5000,
                closable: true,
              });
            }
          })
          .catch(() => {
            this.$buefy.notification.open({
              message: "Error while changing password",
              type: "is-danger",
              duration: 5000,
              closable: true,
            });
          });
        }
    }
    },
    validations: {
    model: {
      oldPassword: {
          required
      },
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
      repeatPassword: {
        required,
        sameAsPassword: sameAs("newPassword"),
      },
    },
  },
}

</script>

<style>

</style>