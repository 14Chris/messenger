<template>
<div>
    <img v-if="imageExists" :src="userPictureUrl" alt="Avatar" class="avatar">
    <div v-else class="avatar avatar-empty"></div>
    <button class="button" >Change</button>
    <input class="input" type="file">
    <button class="button is-danger" v-if="newImage != null || imageExists">Delete</button>
    <button class="button is-primary" v-if="newImage != null">Save</button>
  </div>
</template>

<script>
    import ApiService from "@/service/api";
    const api = new ApiService();
    export default {
        data(){
            return {
                imageExists:false,
                newImage:null
            }
        },
        mounted(){
             
            this.CheckUrlValidity()
        },
        props:["userId"],
        computed:{
            userPictureUrl(){
                return process.env.VUE_APP_API_URL + "/"+this.userId+"/picture"
            }
        },
        methods:{
            CheckUrlValidity(){
                api.headData(this.userPictureUrl)
                .then(response => {
                    this.imageExists = response.ok
                })
            }
        }
    }
</script>

<style>
    .avatar {
  vertical-align: middle;
  width: 100px;
  height: 100px;
  border-radius: 50%;
}

    .avatar-empty{
        background-color: red;
    }
</style>