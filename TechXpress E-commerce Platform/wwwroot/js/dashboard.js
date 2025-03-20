$(document).ready(function () {
    // Initialize DataTables
    /*  $('#productsTable').DataTable();*/
    $('#productsTable').DataTable({
        "pageLength": 10,
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]]
    });
    $('#categoriesTable').DataTable({
        "pageLength": 10,
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]]
    });
    $('#ordersTable').DataTable({
        "pageLength": 10,
        "lengthMenu": [[5, 10, 25, 50, -1], [5, 10, 25, 50, "All"]]
    });

    // Sidebar toggle
    $('#sidebarCollapse').on('click', function () {
        $('#sidebar').toggleClass('active');
    });

    // Tab navigation
    $('a[data-tab]').on('click', function (e) {
        e.preventDefault();
        const targetTab = $(this).data('tab');

        // Update sidebar active state
        $('#sidebar li').removeClass('active');
        $(this).parent().addClass('active');

        // Show/hide content
        $('.tab-content').addClass('d-none');
        $(`#${targetTab}`).removeClass('d-none');
    });

    // Chart.js Configuration
    Chart.defaults.color = '#666';
    Chart.defaults.font.family = 'Arial, sans-serif';

    // Revenue Chart
    const revenueCtx = document.getElementById('revenueChart').getContext('2d');
    new Chart(revenueCtx, {
        type: 'line',
        data: {
            labels: ['Jan', 'Feb', 'Mar', 'Apr', 'May', 'Jun'],
            datasets: [{
                label: 'Revenue',
                data: [30000, 35000, 32000, 40000, 38000, 45000],
                borderColor: '#0167f3',
                tension: 0.4,
                fill: true,
                backgroundColor: 'rgba(1, 103, 243, 0.1)'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true,
                    ticks: {
                        callback: function (value) {
                            return '$' + value.toLocaleString();
                        }
                    }
                }
            }
        }
    });

    // Category Chart
    const categoryCtx = document.getElementById('categoryChart').getContext('2d');
    new Chart(categoryCtx, {
        type: 'doughnut',
        data: {
            labels: ['Electronics', 'Accessories', 'Software', 'Gaming', 'Networking'],
            datasets: [{
                data: [45, 32, 28, 56, 23],
                backgroundColor: [
                    '#0167f3',
                    '#36b9cc',
                    '#1cc88a',
                    '#f6c23e',
                    '#e74a3b'
                ]
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });

    // Order Status Chart
    const orderStatusCtx = document.getElementById('orderStatusChart').getContext('2d');
    new Chart(orderStatusCtx, {
        type: 'pie',
        data: {
            labels: ['Completed', 'Pending', 'Processing', 'Cancelled'],
            datasets: [{
                data: [63, 15, 12, 10],
                backgroundColor: [
                    '#1cc88a',
                    '#f6c23e',
                    '#36b9cc',
                    '#e74a3b'
                ]
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    position: 'bottom'
                }
            }
        }
    });

    // Sales Performance Chart
    const salesCtx = document.getElementById('salesChart').getContext('2d');
    new Chart(salesCtx, {
        type: 'bar',
        data: {
            labels: ['Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'],
            datasets: [{
                label: 'Sales',
                data: [12, 19, 15, 17, 21, 16, 14],
                backgroundColor: '#0167f3'
            }]
        },
        options: {
            responsive: true,
            maintainAspectRatio: false,
            plugins: {
                legend: {
                    display: false
                }
            },
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });




    // Populate tables with sample data
    function populateTable(tableId, data, columns) {
        const table = $(`#${tableId}`).DataTable();
        table.clear();
        data.forEach(item => {
            const row = columns.map(col => item[col]);
            table.row.add(row);
        });
        table.draw();
    }

    // Add event listeners for CRUD operations
    $('.btn-danger').on('click', function () {
        if (confirm('Are you sure you want to delete this item?')) {
            // Handle delete operation
        }
    });

    $('.btn-primary').on('click', function () {
        // Handle edit operation
    });

    // Form validation
    const forms = document.querySelectorAll('.needs-validation');
    Array.from(forms).forEach(form => {
        form.addEventListener('submit', event => {
            if (!form.checkValidity()) {
                event.preventDefault();
                event.stopPropagation();
            }
            form.classList.add('was-validated');
        }, false);
    });

    //// View product details
    //$('.view-product').on('click', function () {
    //    const id = $(this).data('id');
    //    $.get(`/Admin/GetProduct/${id}`, function (data) {
    //        $('#viewProductModal .modal-body').html(data);
    //        $('#viewProductModal').modal('show');
    //    });
    //});

    //// Edit product
    //$('.edit-product').on('click', function () {
    //    const id = $(this).data('id');
    //    $.get(`/Admin/GetProductForm/${id}`, function (data) {
    //        $('#editProductModal .modal-body').html(data);
    //        $('#editProductModal').modal('show');
    //    });
    //});

    //// Delete product
    //$('.delete-product').on('click', function () {
    //    const id = $(this).data('id');
    //    if (confirm('Are you sure you want to delete this product?')) {
    //        $.post(`/Admin/DeleteProduct/${id}`, function () {
    //            location.reload();
    //        });
    //    }
    //});

    //// Edit category
    //$('.edit-category').on('click', function () {
    //    const id = $(this).data('id');
    //    $.get(`/Admin/GetCategoryForm/${id}`, function (data) {
    //        $('#editCategoryModal .modal-body').html(data);
    //        $('#editCategoryModal').modal('show');
    //    });
    //});

    //// Delete category
    //$('.delete-category').on('click', function () {
    //    const id = $(this).data('id');
    //    if (confirm('Are you sure you want to delete this category?')) {
    //        $.post(`/Admin/DeleteCategory/${id}`, function () {
    //            location.reload();
    //        });
    //    }
    //});

    //// View order details
    //$('.view-order').on('click', function () {
    //    const id = $(this).data('id');
    //    $.get(`/Admin/GetOrder/${id}`, function (data) {
    //        $('#viewOrderModal .modal-body').html(data);
    //        $('#viewOrderModal').modal('show');
    //    });
    //});

    //// Update order status
    //$('.update-order-status').on('click', function () {
    //    const id = $(this).data('id');
    //    $('#updateOrderStatusModal [name="Id"]').val(id);
    //    $('#updateOrderStatusModal').modal('show');
    //});



});