export default class ApiService {
    apiUrl = process.env.VUE_APP_API_URL;
    apiKey = "003026bbc133714df1834b8638bb496e-8f4b3d9a-e931-478d-a994-28a725159ab9"

    getData(route) {
        return fetch(this.createCompleteRoute(route, this.apiUrl), {
            method: 'GET',
            headers: this.generateHeaders(),
        });
    }

    headData(route) {
        return fetch(this.createCompleteRoute(route, this.apiUrl), {
            method: 'HEAD',
            headers: this.generateHeaders(),
        });
    }

    create(route, body) {
        return fetch(this.createCompleteRoute(route, this.apiUrl), {
            method: 'POST',
            headers: this.generateHeaders(),
            body: body
        });
    }

    update(route, body) {
        return fetch(this.createCompleteRoute(route, this.apiUrl), {
            method: 'PUT',
            headers: this.generateHeaders(),
            body: body
        });
    }

    delete(route, body) {
        return fetch(this.createCompleteRoute(route, this.apiUrl), {
            method: 'DELETE',
            headers: this.generateHeaders(),
            body: body
        });
    }

    createCompleteRoute(route, envAddress) {
        return `${envAddress}/${route}`;
    }

    generateHeaders() {
        var authToken = localStorage.getItem('token')
        if (authToken && authToken.length > 0) {
            return {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'Authorization': `Bearer ${authToken}`,
                'X-API-KEY': this.apiKey
            }
        }
        else {
            return {
                'Accept': 'application/json',
                'Content-Type': 'application/json',
                'X-API-KEY': this.apiKey
            }
        }
    }
}