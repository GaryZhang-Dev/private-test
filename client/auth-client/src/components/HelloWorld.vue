<template>
  <div class="hello">
    <h1>{{ msg }}</h1>
    <h2>Essential Links</h2>
    <!-- <ul>
      <li><router-link :to='{name:"test"}'>Test</router-link></li>
      <li>
        <router-link :to="{name:'login-password'}">Login</router-link>
      </li>
    </ul>-->
    <router-view></router-view>
  </div>
</template>

<script>
export default {
  name: 'HelloWorld',
  data() {
    return {
      msg: 'Welcome to Your Vue.js App',
    }
  },
  methods: {
    login() {
      this.$router.push({ name: 'login-password' })
    },
  },
}
</script>

<!-- Add "s-*coped" attribute to limit CSS to this component only -->
<style scoped>
h1,
h2 {
  font-weight: normal;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  display: inline-block;
  margin: 0 10px;
}
a {
  color: #42b983;
}
</style>
