import { LitElement, html, css } from '/Scripts/js/libs/lit/lit.js';
import { MxCustomSelect } from './mx-custom-select.js';
import { GetUsers } from '/Scripts/js/Services/common-services.js'

export class MxUserSelect extends MxCustomSelect {
    firstUpdated(){
        super.firstUpdated()
        this.keyLabel = 'Nombre'
        this.keyValue = 'UsuarioId'
        GetUsers()
        .then(json => {
            const users = json.d.Data
            this.items = users.filter(user => user.UsuarioId != 505050)
        })
    }


    renderOption(item){
        return html`
            <div class="user-option">
                <img src="${item.Avatar}" alt="${item.Nombre}" class="user-avatar">
                <div class="user-info">
                    <span class="user-name">${item.Nombre}</span>
                    <span class="user-position">${item.Cargo}</span>
                </div>
            </div>
        `;
    }

    renderSelectedOption(item){
        return html`
        <div class="user-option user-option--selected">
            <img src="${item.Avatar}" alt="${item.Nombre}" class="user-avatar">
            <div class="user-info">
                <span class="user-name">${item.Nombre}</span>
            </div>
        </div>`;
    }

    static styles = [...super.styles, css`
        .user-option{
            display: flex;
            align-items: center;
            gap: .5rem;
        }
        .user-avatar{
            width: 2rem;
            height: 2rem;
            border-radius: 50%;
            object-fit: cover;
            border: 1px solid #E0E3EE;
        }
        .user-info{
            display: flex;
            flex-direction: column;
            gap: .25rem;
        }
        .user-name{
            font-weight: 600;
        }
        .user-position{
            font-size: 12px;
        }
        .user-option--selected .user-avatar{
            width: 1.2rem;
            height: 1.2rem;
        }
    `];
}

customElements.define('mx-user-select', MxUserSelect);
