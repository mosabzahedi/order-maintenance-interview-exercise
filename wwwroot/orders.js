function byId(id) {
    return document.getElementById(id);
}

let loadedId;

function show(message) {
    byId('message').textContent = message;
}

function display(order) {
    byId('customer').textContent = `${order.trackingNumber} — ${order.customerDisplayName}`;
    byId('status').value = order.status;
    byId('note').value = order.customerNote;
}

async function readResponse(response) {
    if (!response.ok) {
        throw new Error(`Request failed (${response.status}). Your changes were not confirmed.`);
    }

    return await response.json();
}

// Load the selected order when the user clicks Load order.
byId('load').addEventListener('click', async () => {
    byId('load').disabled = true;
    byId('fields').disabled = true;
    loadedId = undefined;

    try {
        const id = byId('order-id').value;
        const response = await fetch(`/api/orders/${id}`, { cache: 'no-store' });
        const order = await readResponse(response);

        display(order);
        loadedId = order.id;
        byId('fields').disabled = false;
        show('Order loaded.');
    } catch (error) {
        show(error.message);
    } finally {
        byId('load').disabled = false;
    }
});

// Submit the form and display the values returned by the API.
byId('edit').addEventListener('submit', async event => {
    event.preventDefault();

    if (loadedId === undefined) {
        return;
    }

    const request = {
        status: byId('status').value,
        customerNote: byId('note').value
    };

    // Razor supplies this token to protect the request against cross-site submission.
    const token = document.querySelector('#edit input[name="__RequestVerificationToken"]').value;

    byId('fields').disabled = true;
    byId('load').disabled = true;

    try {
        const response = await fetch(`/api/orders/${loadedId}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json',
                'X-CSRF-TOKEN': token
            },
            body: JSON.stringify(request)
        });

        const order = await readResponse(response);
        display(order);
        show(`Saved. Status: ${order.status}.`);
    } catch (error) {
        show(error.message);
    } finally {
        byId('fields').disabled = false;
        byId('load').disabled = false;
    }
});
