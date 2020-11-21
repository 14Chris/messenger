<template>
  <div class="container">
    <h3 class="title is-3">Edit profile</h3>
    <div v-if="isLoading">
      <h1>Loading...</h1>
    </div>
    <div v-else>
      <div v-if="userProfile != null">
        <AvatarEdit :userId="userProfile.id"></AvatarEdit>

        <!-- Informations -->
        <div class="box"> 
          <form @submit.prevent="UpdateUserInformations">
            <h4 class="title is-4">Informations</h4>

            <div class="columns">
              <div v-if="!$v.userProfile.first_name.$invalid" class="field column">
                <label class="label">First name</label>
                <div class="control">
                  <input
                    class="input"
                    type="text"
                    placeholder="First name"
                    v-model="userProfile.first_name"
                    required
                  />
                </div>
              </div>

              <div v-else class="field column">
                <label class="label">First name</label>
                <div class="control">
                  <input
                    class="input is-danger"
                     type="text"
                    placeholder="First name"
                    v-model="userProfile.first_name"
                  />
                </div>
                <p class="help is-danger">First name is required</p>
              </div>

              <div class="field column">
                <label class="label">Last name</label>
                <div class="control">
                  <input
                    class="input"
                    type="text"
                    placeholder="Last name"
                    v-model="userProfile.last_name"
                  />
                </div>
              </div>
            </div>

            <div class="field">
              <label class="label">Email</label>
              <div class="control">
                <input
                  class="input"
                  type="text"
                  placeholder="Email"
                  v-model="userProfile.email"
                  disabled
                  readonly="true"
                />
              </div>
            </div>

            <button class="button is-primary">Save</button>
          </form>
        </div>

        <!-- Security -->
         <div class="box">
            <h4 class="title is-4">Security</h4>
            <ChangePassword></ChangePassword>
        </div>
      </div>
      <div v-else>
        <p>An error occured while retrieving user profile</p>
      </div>
    </div>
  </div>
</template>

<script>
import { required } from "vuelidate/lib/validators";
import ApiService from "@/service/api";
import AvatarEdit from "@/components/User/Avatar/AvatarEdit";
import ChangePassword from "@/components/User/ChangePassword";

const api = new ApiService();
export default {
  components: {
    AvatarEdit,
    ChangePassword
  },
  data() {
    return {
      userProfile: null,
      isLoading: false,
    };
  },
  mounted() {
    this.GetUserProfileById();
  },
  methods: {
    GetUserProfileById() {
      this.isLoading = true;
      api
        .getData("users/profile")
        .then((response) => {
          console.log(response);
          if (response.ok == true) {
            response.json().then((data) => {
              if (data.ResponseType == 1) {
                this.userProfile = data.Result;
              } else {
                this.userProfile = null;
              }
            });
          }
        })
        .catch((err) => {
          console.log(err);
        })
        .finally(() => {
          this.isLoading = false;
        });
    },
    UpdateUserInformations() {
      if (!this.$v.$invalid) {
        api
          .update("users", JSON.stringify(this.userProfile))
          .then((response) => {
            console.log(response);
            if (response.ok == true) {
              response.json().then((data) => {
                if (data.ResponseType == 1) {
                  this.userProfile = data.Result;
                  this.$buefy.notification.open({
                    message: "Your informations have been updated",
                    type: "is-success",
                    duration: 5000,
                    closable: true,
                  });
                } else {
                  this.$buefy.notification.open({
                    message: "Error while updating user informations",
                    type: "is-danger",
                    duration: 5000,
                    closable: true,
                  });
                }
              });
            } else {
              this.$buefy.notification.open({
                message: "Error while updating user informations",
                type: "is-danger",
                duration: 5000,
                closable: true,
              });
            }
          })
          .catch(() => {
            this.$buefy.notification.open({
              message: "Error while updating user informations",
              type: "is-danger",
              duration: 5000,
              closable: true,
            });
          })
          .finally(() => {
            this.isLoading = false;
          });
      }
    },
  },
  validations: {
    userProfile: {
      first_name: {
        required,
      },
    },
  },
};
</script>

<style scoped>
.box:first{
  margin-top: 25px;
}

.box{
   margin-bottom: 25px;
}
</style>